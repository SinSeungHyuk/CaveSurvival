using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnWeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponImage;

    private Button btnWeapon;


    private void Awake()
    {
    }

    public void InitializeBtnWeaponUI(WeaponDetailsSO weaponData, WeaponUpgradeView weaponUpgradeView, CurrencySystem currencySystem, UpgradeSystem upgradeSystem)
    {
        weaponImage.sprite = weaponData.weaponSprite;
        btnWeapon = GetComponent<Button>();

        btnWeapon.onClick.RemoveAllListeners();
        btnWeapon.onClick.AddListener(()
            => InitialzieWeaponUpgradeView(weaponData, weaponUpgradeView, currencySystem, upgradeSystem));
    }

    private void InitialzieWeaponUpgradeView(WeaponDetailsSO weaponData, WeaponUpgradeView weaponUpgradeView, CurrencySystem currencySystem, UpgradeSystem upgradeSystem)
    {
        weaponUpgradeView.InitializeWeaponUpgradeView(weaponData, currencySystem, upgradeSystem);
    }
}
