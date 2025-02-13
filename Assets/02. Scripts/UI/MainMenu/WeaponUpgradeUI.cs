using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtWeaponStat;
    [SerializeField] private TextMeshProUGUI txtUpgradeValue;
    [SerializeField] private TextMeshProUGUI txtGold;
    [SerializeField] private Button btnUpgrade;


    public void InitializeWeaponUpgradeUI(int upgradeIndex, WeaponDetailsSO weaponData, CurrencySystem currencySystem, UpgradeSystem upgradeSystem)
    {
        SetTxtWeaponUpgradeUI(upgradeIndex, weaponData);

        btnUpgrade.onClick.RemoveAllListeners();
        btnUpgrade.onClick.AddListener(() 
            => UpgradeWeapon(upgradeIndex, weaponData, currencySystem, upgradeSystem));
    }

    private void UpgradeWeapon(int upgradeIndex, WeaponDetailsSO weaponData, CurrencySystem currencySystem, UpgradeSystem upgradeSystem)
    {
        upgradeSystem.UpgradeWeapon(upgradeIndex,weaponData, currencySystem);

        SetTxtWeaponUpgradeUI(upgradeIndex, weaponData);
    }

    private void SetTxtWeaponUpgradeUI(int upgradeIndex, WeaponDetailsSO weaponData)
    {
        switch (upgradeIndex)
        {
            case 0:
                txtWeaponStat.text = $"[ {weaponData.weaponBaseDamage} ]";
                txtUpgradeValue.text = $"+ {Settings.weaponDamageUpgrade}";
                txtGold.text = Settings.weaponDamageUpgradeGold.ToString("N0");
                break;
            case 1:
                txtWeaponStat.text = $"[ {weaponData.weaponCriticChance} ]";
                txtUpgradeValue.text = $"+ {Settings.weaponCriticChanceUpgrade}";
                txtGold.text = Settings.weaponCriticChanceUpgradeGold.ToString("N0");
                break;
            case 2:
                txtWeaponStat.text = $"[ {weaponData.weaponCriticDamage} ]";
                txtUpgradeValue.text = $"+ {Settings.weaponCriticDamageUpgrade}";
                txtGold.text = Settings.weaponCriticDamageUpgradeGold.ToString("N0");
                break;
            case 3:
                txtWeaponStat.text = $"[ {weaponData.weaponFireRate.ToString("F2")}/s ]";
                txtUpgradeValue.text = $"+ {Settings.weaponFireRateUpgrade}%";
                txtGold.text = Settings.weaponFireRateUpgradeGold.ToString("N0");
                break;
            case 4:
                txtWeaponStat.text = $"[ {weaponData.weaponRange} ]";
                txtUpgradeValue.text = $"+ {Settings.weaponRangeUpgrade}%";
                txtGold.text = Settings.weaponRangeUpgradeGold.ToString("N0");
                break;
            case 5:
                txtWeaponStat.text = $"[ {weaponData.weaponKnockback} ]";
                txtUpgradeValue.text = $"+ {Settings.weaponKnockbackUpgrade}";
                txtGold.text = Settings.weaponKnockbackUpgradeGold.ToString("N0");
                break;
        }
    }
}
