using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class ProjectileEffectSO : ScriptableObject
{
    protected List<BonusEffectSO> bonusEffects = new List<BonusEffectSO>();
    private HitEffect hitEffect;

    // 투사체가 가질 효과

    public void InitializePE(List<BonusEffectSO> bonusEffects)
        => this.bonusEffects = bonusEffects;

    protected void HitEffectRendering(Monster monster, Weapon weapon)
    {
        if (monster.isOutScreen == false)
        {
            hitEffect = ObjectPoolManager.Instance.Get(weapon.WeaponParticle, monster.transform.position, Quaternion.identity).GetComponent<HitEffect>();
            hitEffect.InitializeHitEffect(weapon.WeaponParticle);
        }
    }

    public virtual void Start() { }
    public abstract void Apply(Monster monster, Weapon weapon, Vector2 direction);
    public virtual void Release() { }
}
