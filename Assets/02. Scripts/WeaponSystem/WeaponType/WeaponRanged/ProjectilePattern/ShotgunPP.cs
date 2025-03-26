using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShotgunPP_", menuName = "Scriptable Objects/Weapon/Projectile/PP/Shotgun")]
public class ShotgunPP : ProjectilePatternSO
{
    // 투사체 오브젝트
    private GameObject projectileObject;

    [SerializeField] private int shotgunAngle; // 샷건 각도
    [SerializeField] private int projectileCount; // 동시에 발사할 탄의 수 (**** 한쪽 방향만 계산. 즉, 3발을 쏘고싶으면 1로 설정)

    

    public override void ProjectileLaunch(ProjectileData projectileData, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        // 가운데 중심 탄환
        projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
        projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, direction, weapon);

        // 각도에 i를 곱한만큼 일정한 각도로 회전된 탄 발사
        for (int i = 1; i <= projectileCount; i++)
        {
            projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
            projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, RotateDirection(direction, (shotgunAngle * i)), weapon);

            projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
            projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, RotateDirection(direction, -(shotgunAngle * i)), weapon);
        }
    }

    private Vector2 RotateDirection(Vector2 direction, float angleDegree)
    {
        float angleRad = angleDegree * Mathf.Deg2Rad; // 라디안으로 변환
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);

        return new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        );
    }
}