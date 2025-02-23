using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIController : MonoBehaviour
{
    [SerializeField] private WeaponListView weaponListView;
    [SerializeField] private WeaponUpgradeView weaponUpgradeView;
    [SerializeField] private Button btnExit;


    private void Awake()
    {
        btnExit.onClick.RemoveAllListeners();
        btnExit.onClick.AddListener(ExitUpgradeView);
    }

    public void InitializeUpgradeUIController(CurrencySystem currencySystem, UpgradeSystem upgradeSystem)
    {
        weaponListView.InitializeWeaponListView(weaponUpgradeView, currencySystem, upgradeSystem);
        weaponUpgradeView.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(true);
    }


    private void ExitUpgradeView()
    {
        weaponUpgradeView.gameObject.SetActive(false);
        weaponListView.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);
    }
}
