using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShotgunPP_", menuName = "Scriptable Objects/Weapon/Projectile/PP/Shotgun")]
public class ShotgunPP : ProjectilePatternSO
{
    // ����ü ������Ʈ
    private GameObject projectileObject;

    [SerializeField] private int shotgunAngle; // ���� ����
    [SerializeField] private int projectileCount; // ���ÿ� �߻��� ź�� �� (**** ���� ���⸸ ���. ��, 3���� �������� 1�� ����)

    

    public override void ProjectileLaunch(ProjectileData projectileData, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon)
    {
        // ��� �߽� źȯ
        projectileObject = ObjectPoolManager.Instance.Get(EPool.Projectile, weapon.Player.WeaponTransform.GetWeaponTransform(weapon));
        projectileObject.GetComponent<Projectile>().InitializeProjectile(projectileData, bonusEffects, direction, weapon);

        // ������ i�� ���Ѹ�ŭ ������ ������ ȸ���� ź �߻�
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
        float angleRad = angleDegree * Mathf.Deg2Rad; // �������� ��ȯ
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);

        return new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        );
    }
}