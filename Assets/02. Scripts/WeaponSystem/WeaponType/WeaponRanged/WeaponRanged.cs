using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WR_", menuName = "Scriptable Objects/Weapon/Ranged")]
public class WeaponRanged : WeaponTypeDetailsSO
{
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private ProjectilePatternSO pattern;
    [SerializeField] private List<BonusEffectSO> bonusEffects; // ����ü�� ���� �߰�ȿ�� (�����, ��Ʈ������ ��)


    public override void Attack(Weapon weapon)
    {
        if (weapon.DetectorType.DetectMonster(weapon, out Vector2 direction, out Monster monster) == false)
            return; // ��Ÿ� ���� �� ã�� (Ž�� ������ ���⸶�� ��������)

        // �� ���Ÿ����Ⱑ ���� �߻����ϴ�� ����ü �߻�
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