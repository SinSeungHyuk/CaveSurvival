using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkDebuff_", menuName = "Scriptable Objects/Weapon/BonusEffect/AttackDebuff")]
public class AttackDebuffBE : BonusEffectSO
{
    [SerializeField] private int atkDebuffValue;
    [SerializeField] private int atkDebuffDuration;


    public override void Apply(Monster monster)
    {
        monster.MonsterState.AddState(EDebuffType.AttackDebuff, atkDebuffValue, atkDebuffDuration);
    }
}