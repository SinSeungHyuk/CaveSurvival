using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


[CreateAssetMenu(fileName = "WM_", menuName = "Scriptable Objects/Weapon/Melee")]
public class WeaponMelee : WeaponTypeDetailsSO
{
    [SerializeField] private List<BonusEffectSO> bonusEffects;
    [SerializeField] private int meleeRange = 3;
    private HitEffect hitEffect;



    public override void Attack(Weapon weapon)
    {
        if (weapon.DetectorType.DetectMonster(weapon, out Vector2 direction, out Monster monster) == false)
            return; // ��Ÿ� ���� �� ã�� (Ž�� ������ ���⸶�� ��������)

        var colliders = Physics2D.OverlapCircleAll(monster.transform.position, meleeRange, Settings.monsterLayer);

        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<Monster>();
            target.TakeDamage(weapon, weapon.Player.Stat.MeleeDamage);
            target.Rigid.AddForce(direction * weapon.WeaponKnockback);

            HitEffectRendering(target, weapon);

            foreach (var effect in bonusEffects)
            {
                effect.Apply(target);
            }
        }

        PostAttack(weapon, direction);
    }

    private void PostAttack(Weapon weapon, Vector2 direction)
    {
        // �������� : ���� ��ġ�� ���� ȸ�� �� ��� ���(�̵�)
        float angle = UtilitieHelper.GetAngleFromVector(direction);
        weapon.Player.WeaponTransform.RotateWeapon(weapon, angle);
        weapon.Player.WeaponTransform.MoveWeapon(weapon, direction);
        SoundEffectManager.Instance.PlaySoundEffect(weapon.WeaponSoundEffect);
    }

    private void HitEffectRendering(Monster monster, Weapon weapon)
    {
        if (monster.isOutScreen == false)
        {
            hitEffect = ObjectPoolManager.Instance.Get(weapon.WeaponParticle, monster.transform.position, Quaternion.identity).GetComponent<HitEffect>();
            hitEffect.InitializeHitEffect(weapon.WeaponParticle);
        }
    }
}
