using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossPP_", menuName = "Scriptable Objects/Weapon/Projectile/PP/Cross")]
public class CrossPP : ProjectilePatternSO
{
    // ����ü ������Ʈ
    private GameObject projectileObject;

    private List<Func<Vector2, Vector2>> crossDirections = new List<Func<Vector2, Vector2>>
    {
        direction => new Vector2(-direction.y, direction.x), // ���͸� 90�� ȸ��
        direction => new Vector2(direction.y, -direction.x), // ���͸� -90�� ȸ��
        direction => new Vector2(-direction.x, -direction.y), // ���͸� 180�� ȸ��
        direction => direction                       // ���� ���͸� ��ȯ
    };

    public override void ProjectileLaunch(ProjectileDetailsSO projectileDetails, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        for (int i = 0;i < crossDirections.Count; i++)
        {
            projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
            projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileDetails, bonusEffects, crossDirections[i].Invoke(direction), weapon);
        }
    }
}