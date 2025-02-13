using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponListView : MonoBehaviour
{
    [SerializeField] private BtnWeaponUI btnWeaponUI;
    [SerializeField] private Transform contentsTransform;


    private void OnDisable()
    {
        foreach (Transform btn in contentsTransform)
        {
            // Destroy는 해당 프레임에서 파괴하는 것이 아니라 파괴 '예약'이기 때문에 foreach 가능
            Destroy(btn.gameObject);
        }
    }

    public void InitializeWeaponListView(WeaponUpgradeView weaponUpgradeView, CurrencySystem currencySystem, UpgradeSystem upgradeSystem)
    {
        Database weaponDB = AddressableManager.Instance.GetResource<Database>("DB_Weapon");

        for (int i = 0; i < weaponDB.Count; i++)
        {
            var weaponData = weaponDB.GetDataByID<WeaponDetailsSO>(i);
            var btnWeapon = Instantiate(btnWeaponUI, contentsTransform).GetComponent<BtnWeaponUI>();
            btnWeapon.InitializeBtnWeaponUI(weaponData, weaponUpgradeView, currencySystem, upgradeSystem);
        }


        gameObject.SetActive(true);
    }

}
