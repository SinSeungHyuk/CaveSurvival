using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeView : MonoBehaviour
{
    [SerializeField] private Image weaponImage; 
    [SerializeField] private TextMeshProUGUI txtWeaponName;
    [SerializeField] private List<WeaponUpgradeUI> weaponUpgradeList = new List<WeaponUpgradeUI>();


    public void InitializeWeaponUpgradeView(WeaponDetailsSO weaponData, CurrencySystem currencySystem, UpgradeSystem upgradeSystem)
    {
        weaponImage.sprite = weaponData.weaponSprite;
        txtWeaponName.text = weaponData.weaponName;

        Debug.Log($"WeaponUpgradeView - InitializeWeaponUpgradeView - {weaponData.weaponName}");

        for (int i = 0; i < weaponUpgradeList.Count; i++)
        {
            weaponUpgradeList[i].InitializeWeaponUpgradeUI(i, weaponData, currencySystem, upgradeSystem);
        }

        gameObject.SetActive(true);
    }
}
