using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoFirePP_", menuName = "Scriptable Objects/Weapon/Projectile/PP/AutoFire")]
public class AutoFirePP : ProjectilePatternSO
{
    // 투사체 오브젝트
    private GameObject projectileObject;

    [SerializeField] private int addFireCount; // 추가로 연사할 탄의 수


    public override void ProjectileLaunch(ProjectileData projectileData, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
        projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, direction, weapon);

        AutoFire(projectileData, bonusEffects, direction, weapon).Forget();
    }

    private async UniTask AutoFire(ProjectileData projectileData, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        for (int i = 0; i< addFireCount; i++)
        {
            await UniTask.Delay(50);

            projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
            projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, direction, weapon);
        }
    }
}
