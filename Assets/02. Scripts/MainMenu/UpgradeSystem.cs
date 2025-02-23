using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class UpgradeSystem : MonoBehaviour, ISaveData
{
    // 저장,불러오기를 위한 딕셔너리 <무기 id, List<스탯별 레벨>>
    private Dictionary<int, List<int>> upgradeDic = new Dictionary<int, List<int>>();
    private Database weaponDB;

    public IReadOnlyDictionary<int, List<int>> UpgradeDic => upgradeDic;


    
    void Start()
    {
        weaponDB = AddressableManager.Instance.GetResource<Database>("DB_Weapon");

        for (int i = 0; i < weaponDB.Count; i++)
        {
            upgradeDic.Add(i, Settings.upgradgeDefaultLevel);
        }

        Register();
    }

    private void InitializeUpgradeWeapon(WeaponDetailsSO weaponDetailsSO, List<int> levelList)
    {
        // 업그레이드 데이터를 각 무기DB에 반영하기 (스탯별로 다르게 적용)
        weaponDetailsSO.weaponBaseDamage += (levelList[0] * Settings.weaponDamageUpgrade);
        weaponDetailsSO.weaponCriticChance += (levelList[1] * Settings.weaponCriticChanceUpgrade);
        weaponDetailsSO.weaponCriticDamage += (levelList[2] * Settings.weaponCriticDamageUpgrade);
        weaponDetailsSO.weaponFireRate = UtilitieHelper.IncreaseByPercent(weaponDetailsSO.weaponFireRate, (levelList[3] * Settings.weaponFireRateUpgrade));
        weaponDetailsSO.weaponRange = UtilitieHelper.IncreaseByPercent(weaponDetailsSO.weaponRange, (levelList[4] * Settings.weaponRangeUpgrade));
        weaponDetailsSO.weaponKnockback += (levelList[5] * Settings.weaponKnockbackUpgrade);
    }

    public void UpgradeWeapon(int upgradeIndex, WeaponDetailsSO weaponDetailsSO, CurrencySystem currencySystem)
    {
        int weaponID = weaponDetailsSO.ID;

        // 업그레이드 가능한지 확인
        if (!CanUpgrade(currencySystem, GetUpgradeCost(upgradeIndex)))
            return;

        // 업그레이드 적용
        ApplyUpgrade(upgradeIndex, weaponDetailsSO);
        upgradeDic[weaponID][upgradeIndex]++;
    }

    private bool CanUpgrade(CurrencySystem currencySystem, int gold)
    {
        if (currencySystem.GetCurrency(ECurrencyType.Gold) >= gold)
        {
            currencySystem.IncreaseCurrency(ECurrencyType.Gold, -gold);
            return true;
        }
        return false;
    }

    private int GetUpgradeCost(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 0: return Settings.weaponDamageUpgradeGold;
            case 1: return Settings.weaponCriticChanceUpgradeGold;
            case 2: return Settings.weaponCriticDamageUpgradeGold;
            case 3: return Settings.weaponFireRateUpgradeGold;
            case 4: return Settings.weaponRangeUpgradeGold;
            case 5: return Settings.weaponKnockbackUpgradeGold;
            default: throw new ArgumentOutOfRangeException(nameof(upgradeIndex), "유효하지 않은 인덱스!!");
        }
    }

    private void ApplyUpgrade(int upgradeIndex, WeaponDetailsSO weaponDetailsSO)
    {
        switch (upgradeIndex)
        {
            case 0:
                weaponDetailsSO.weaponBaseDamage += Settings.weaponDamageUpgrade;
                break;
            case 1:
                weaponDetailsSO.weaponCriticChance += Settings.weaponCriticChanceUpgrade;
                break;
            case 2:
                weaponDetailsSO.weaponCriticDamage += Settings.weaponCriticDamageUpgrade;
                break;
            case 3:
                weaponDetailsSO.weaponFireRate = UtilitieHelper.IncreaseByPercent(weaponDetailsSO.weaponFireRate, Settings.weaponFireRateUpgrade);
                break;
            case 4:
                weaponDetailsSO.weaponRange = UtilitieHelper.IncreaseByPercent(weaponDetailsSO.weaponRange, Settings.weaponRangeUpgrade);
                break;
            case 5:
                weaponDetailsSO.weaponKnockback += Settings.weaponKnockbackUpgrade;
                break;
        }
    }


    #region SAVE & LOAD
    public void Register()
    {
        SaveManager.Instance.Register(this);
    }
    public void ToSaveData()
    {
        // 딕셔너리 직렬화를 위한 SerializeDictionary 객체 생성 => 딕셔너리를 두개의 리스트로 쪼개기
        SerializeDictionary<int, List<int>> serializeDictionary = new SerializeDictionary<int,List<int>>(upgradeDic);

        UpgradeSaveData unlockSaveData = new UpgradeSaveData()
        {
            weaponIdList = serializeDictionary.Keys,
            upgradeLevelList = serializeDictionary.Values
        };

        SaveManager.Instance.SaveData.UpgradeData = unlockSaveData;
    }
    public void FromSaveData(SaveData saveData)
    {
        var upgradeLevelList = saveData.UpgradeData.upgradeLevelList;

        for (int i = 0; i < upgradeLevelList.Count; i++)
        {
            upgradeDic[i] = upgradeLevelList[i];

            InitializeUpgradeWeapon(weaponDB.GetDataByID<WeaponDetailsSO>(i), upgradeDic[i]);
        }
    }
    #endregion
}
