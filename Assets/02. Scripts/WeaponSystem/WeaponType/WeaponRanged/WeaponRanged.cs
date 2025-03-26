using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WR_", menuName = "Scriptable Objects/Weapon/Ranged")]
public class WeaponRanged : WeaponTypeDetailsSO
{
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private ProjectilePatternSO pattern;
    [SerializeField] private List<BonusEffectSO> bonusEffects; // 투사체에 붙을 추가효과 (디버프, 도트데미지 등)


    public override void Attack(Weapon weapon)
    {
        if (weapon.DetectorType.DetectMonster(weapon, out Vector2 direction, out Monster monster) == false)
            return; // 사거리 내의 적 찾기 (탐지 기준은 무기마다 설정가능)

        // 이 원거리무기가 가진 발사패턴대로 투사체 발사
        pattern.ProjectileLaunch(projectileData, bonusEffects, direction, weapon);

        PostAttack(weapon, direction);
    }

    private void PostAttack(Weapon weapon, Vector2 direction)
    {
        float angle = UtilitieHelper.GetAngleFromVector(direction);
        weapon.Player.WeaponTransform.RotateWeapon(weapon, angle);
        SoundEffectManager.Instance.PlaySoundEffect(weapon.WeaponSoundEffect);
    }
}