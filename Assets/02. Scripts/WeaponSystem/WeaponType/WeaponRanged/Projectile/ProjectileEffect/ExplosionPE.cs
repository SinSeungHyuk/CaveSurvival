using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ExplosionPE_", menuName = "Scriptable Objects/Weapon/Projectile/PE/Explosion")]
public class ExplosionPE : ProjectileEffectSO
{
    [SerializeField] private int explosionRange;

    public override void Apply(Monster monster, Weapon weapon, Vector2 direction)
    {
        var colliders = Physics2D.OverlapCircleAll(monster.transform.position, explosionRange, Settings.monsterLayer);

        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<Monster>();
            target.TakeDamage(weapon, weapon.Player.Stat.RangeDamage);
            target.Rigid.AddForce(direction * weapon.WeaponKnockback);

            HitEffectRendering(target, weapon);

            foreach (var effect in bonusEffects)
            {
                effect.Apply(target);
            }
        }
    }
}
