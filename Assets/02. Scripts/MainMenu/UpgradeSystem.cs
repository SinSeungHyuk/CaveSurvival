using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UpgradeSystem : MonoBehaviour, ISaveData
{
    // 저장,불러오기를 위한 딕셔너리 <무기 id, List<스탯별 레벨>>
    private SerializableDictionary<int, List<int>> upgradeDic = new SerializableDictionary<int, List<int>>();
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

    private void OnEnable()
    {
        SaveManager.Instance.OnLoadFinished += SaveManager_OnLoadFinished;
    }
    private void OnDisable()
    {
        SaveManager.Instance.OnLoadFinished -= SaveManager_OnLoadFinished;
    }


    private void SaveManager_OnLoadFinished()
    {
        Debug.Log("업글 이벤트 호출, 로드가 끝났다는 뜻");

        for (int i = 0; i < weaponDB.Count; i++)
        {
            InitializeUpgradeWeapon(weaponDB.GetDataByID<WeaponDetailsSO>(i), upgradeDic[i]);
        }
    }

    private void InitializeUpgradeWeapon(WeaponDetailsSO weaponDetailsSO, List<int> levelList)
    {
         weaponDetailsSO.weaponBaseDamage += (levelList[0] * Settings.weaponDamageUpgrade);
         weaponDetailsSO.weaponCriticChance += (levelList[1] * Settings.weaponCriticChanceUpgrade);
         weaponDetailsSO.weaponCriticDamage += (levelList[2] * Settings.weaponCriticDamageUpgrade);
         weaponDetailsSO.weaponFireRate = UtilitieHelper.IncreaseByPercent(weaponDetailsSO.weaponFireRate,(levelList[3] * Settings.weaponFireRateUpgrade));
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


    public void Register()
    {
        SaveManager.Instance.Register(this);
    }
    public void ToSaveData()
    {
        UpgradeSaveData unlockSaveData = new UpgradeSaveData()
        {
            upgradeDic = upgradeDic,
        };

        SaveManager.Instance.SaveData.UpgradeData = unlockSaveData;
    }
    public void FromSaveData(SaveData saveData)
    {
        this.upgradeDic = saveData.UpgradeData.upgradeDic;
    }
}
