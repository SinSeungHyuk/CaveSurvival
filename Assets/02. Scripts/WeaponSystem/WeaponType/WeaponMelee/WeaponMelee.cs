using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WM_", menuName = "Scriptable Objects/Weapon/Melee")]
public class WeaponMelee : WeaponTypeDetailsSO
{
    [SerializeField] private List<BonusEffectSO> bonusEffects;
    [SerializeField] private int meleeRange = 3;
    private HitEffect hitEffect;



    public override void Attack(Weapon weapon)
    {
        if (weapon.DetectorType.DetectMonster(weapon, out Vector2 direction, out GameObject monster) == false)
            return; // ��Ÿ� ���� �� ã�� (Ž�� ������ ���⸶�� ��������)

        var colliders = Physics2D.OverlapCircleAll(monster.transform.position, meleeRange, Settings.monsterLayer);

        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<Monster>();
            target.TakeDamage(weapon, weapon.Player.Stat.MeleeDamage);
            target.Rigid.AddForce(direction * weapon.WeaponKnockback);

            // TEST
            hitEffect = ObjectPoolManager.Instance.Get(weapon.WeaponParticle, target.transform.position, Quaternion.identity).GetComponent<HitEffect>();
            hitEffect.InitializeHitEffect(weapon.WeaponParticle);

            foreach (var effect in bonusEffects)
            {
                effect.Apply(target);
            }
        }

        // �������� : ���� ��ġ�� ���� ȸ�� �� ��� ���(�̵�)
        float angle = UtilitieHelper.GetAngleFromVector(direction);
        weapon.Player.WeaponTransform.RotateWeapon(weapon, angle);
        weapon.Player.WeaponTransform.MoveWeapon(weapon, direction);
    }
}
