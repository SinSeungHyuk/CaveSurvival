using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossPP_", menuName = "Scriptable Objects/Weapon/Projectile/PP/Cross")]
public class CrossPP : ProjectilePatternSO
{
    // 투사체 오브젝트
    private GameObject projectileObject;

    private List<Func<Vector2, Vector2>> crossDirections = new List<Func<Vector2, Vector2>>
    {
        direction => new Vector2(-direction.y, direction.x), // 벡터를 90도 회전
        direction => new Vector2(direction.y, -direction.x), // 벡터를 -90도 회전
        direction => new Vector2(-direction.x, -direction.y), // 벡터를 180도 회전
        direction => direction                       // 원래 벡터를 반환
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