using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class ProjectileEffectSO : ScriptableObject
{
    protected List<BonusEffectSO> bonusEffects = new List<BonusEffectSO>();

    // ����ü�� ���� ȿ��

    public void InitializePE(List<BonusEffectSO> bonusEffects)
        => this.bonusEffects = bonusEffects;

    public virtual void Start() { }
    public abstract void Apply(Monster monster, Weapon weapon, Vector2 direction);
    public virtual void Release() { }
}
