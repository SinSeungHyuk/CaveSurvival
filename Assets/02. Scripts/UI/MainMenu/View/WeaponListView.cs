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
            // Destroy�� �ش� �����ӿ��� �ı��ϴ� ���� �ƴ϶� �ı� '����'�̱� ������ foreach ����
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
