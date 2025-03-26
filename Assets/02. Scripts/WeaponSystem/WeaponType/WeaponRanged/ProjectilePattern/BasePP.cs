using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BasePP_", menuName = "Scriptable Objects/Weapon/Projectile/PP/Base")]
public class BasePP : ProjectilePatternSO
{
    // 투사체 오브젝트
    private GameObject projectileObject;


    public override void ProjectileLaunch(ProjectileData projectileData, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        // 발사 명령이 떨어지면 풀에서 투사체 활성화

        //weapon.Player.WeaponTransform.GetWeaponTransform(weapon, out Vector2 pos, out Quaternion rot);
        projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
        // 투사체 데이터SO랑 방향, 무기 정보 넣어서 초기화
        projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, direction, weapon);
    }
}