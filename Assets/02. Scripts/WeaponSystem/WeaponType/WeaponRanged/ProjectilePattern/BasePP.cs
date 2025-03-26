using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BasePP_", menuName = "Scriptable Objects/Weapon/Projectile/PP/Base")]
public class BasePP : ProjectilePatternSO
{
    // ����ü ������Ʈ
    private GameObject projectileObject;


    public override void ProjectileLaunch(ProjectileData projectileData, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        // �߻� ����� �������� Ǯ���� ����ü Ȱ��ȭ

        //weapon.Player.WeaponTransform.GetWeaponTransform(weapon, out Vector2 pos, out Quaternion rot);
        projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
        // ����ü ������SO�� ����, ���� ���� �־ �ʱ�ȭ
        projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, direction, weapon);
    }
}