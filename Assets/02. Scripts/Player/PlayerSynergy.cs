using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSynergy : MonoBehaviour
{
    private Player player;

    // 보유하고 있는 모든 시너지 카운팅
    private Dictionary<ESynergyType, int> weaponSynergyDic = new Dictionary<ESynergyType, int>();
    // 현재 실제로 적용중인 시너지 리스트
    private List<SynergyDetailsSO> synergyList = new List<SynergyDetailsSO>();

    public IReadOnlyList<SynergyDetailsSO> SynergyList => synergyList;


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void AddWeaponToPlayer(WeaponDetailsSO weaponData)
    {
        FirstAddWeaponToPlayer(weaponData);
        weaponSynergyDic[weaponData.weaponElement]++;
        weaponSynergyDic[weaponData.weaponAttackType]++;

        CanApplySynergy(weaponData);
    }

    private void FirstAddWeaponToPlayer(WeaponDetailsSO weaponData)
    {
        if (!weaponSynergyDic.TryGetValue(weaponData.weaponElement, out _))
            weaponSynergyDic[weaponData.weaponElement] = 0;
        if (!weaponSynergyDic.TryGetValue(weaponData.weaponAttackType, out _))
            weaponSynergyDic[weaponData.weaponAttackType] = 0;
    }

    private void CanApplySynergy(WeaponDetailsSO weaponData)
    {
        Database synergyDB = AddressableManager.Instance.GetResource<Database>("DB_Synergy");

        foreach (var data in synergyDB.DB)
        {
            var synergyData = data as SynergyDetailsSO;

            if (synergyData.synergyType == weaponData.weaponElement
                && synergyData.synergyCount == weaponSynergyDic[weaponData.weaponElement])
            {
                ReleaseSynergy(synergyData);
                ApplySynergy(synergyData);
            }
            if (synergyData.synergyType == weaponData.weaponAttackType
                && synergyData.synergyCount == weaponSynergyDic[weaponData.weaponAttackType])
            {
                ReleaseSynergy(synergyData);
                ApplySynergy(synergyData);
            }
        }
    }

    private void ApplySynergy(SynergyDetailsSO synergyData)
    {
        ApplySynergyToWeapon(synergyData, true);

        synergyList = synergyList.Where(data => data.synergyType != synergyData.synergyType).ToList();
        synergyList.Add(synergyData);
    }

    private void ReleaseSynergy(SynergyDetailsSO synergyData)
    {
        var prevSynergy = synergyList.FirstOrDefault(data => data.synergyType == synergyData.synergyType);
        if (prevSynergy == null)
            return;

        ApplySynergyToWeapon(prevSynergy, false);
    }

    private void ApplySynergyToWeapon(SynergyDetailsSO synergyData, bool isIncrease)
    {
        List<Weapon> weaponList = player.WeaponList;
        int sign = isIncrease ? 1 : -1;

        foreach (Weapon weapon in weaponList)
        {
            if (weapon.WeaponDetails.weaponElement == synergyData.synergyType)
            {
                weapon.WeaponStatChanged(synergyData.synergyStatType, sign * synergyData.synergyValue);
            }
            if (weapon.WeaponDetails.weaponAttackType == synergyData.synergyType)
            {
                weapon.WeaponStatChanged(synergyData.synergyStatType, sign * synergyData.synergyValue);
            }
        }
    }
}
