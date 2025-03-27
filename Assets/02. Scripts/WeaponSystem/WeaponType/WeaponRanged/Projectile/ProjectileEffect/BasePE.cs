using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BasePE_", menuName = "Scriptable Objects/Weapon/Projectile/PE/Base")]
public class BasePE : ProjectileEffectSO
{
    public override void Apply(Monster monster, Weapon weapon, Vector2 direction)
    {
        monster.TakeDamage(weapon, weapon.Player.Stat.RangeDamage);
        monster.Rigid.AddForce(direction * weapon.WeaponKnockback);

        HitEffectRendering(monster, weapon);

        foreach (var effect in bonusEffects)
        {
            effect.Apply(monster);
        }
    }
}
