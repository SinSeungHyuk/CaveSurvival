using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private WeaponAttackEvent weaponAttackEvent;
    private Weapon weapon;


    private void Awake()
    {
        weaponAttackEvent = GetComponent<WeaponAttackEvent>();
    }
    private void OnEnable()
    {
        weaponAttackEvent.OnWeaponAttack += WeaponAttackEvent_OnWeaponAttack;
    }
    private void OnDisable()
    {
        weaponAttackEvent.OnWeaponAttack -= WeaponAttackEvent_OnWeaponAttack;
    }



    private void WeaponAttackEvent_OnWeaponAttack(WeaponAttackEvent arg1, WeaponAttackEventArgs args)
    {
        weapon = args.weapon;
        OnWeaponAttack(args);
    }

    private void OnWeaponAttack(WeaponAttackEventArgs weaponAttackEventArgs)
    {
        // ���ݼӵ�(��Ÿ��) �����Ҷ� ���� �õ�
        if (weapon.WeaponFireRateTimer > 0f)
            return;

        weapon.WeaponType.Attack(weapon);
    }
}
