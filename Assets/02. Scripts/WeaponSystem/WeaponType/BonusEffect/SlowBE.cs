using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Slow_", menuName = "Scriptable Objects/Weapon/BonusEffect/Slow")]
public class SlowBE : BonusEffectSO
{
    [SerializeField] private int slowValue;
    [SerializeField] private int slowDuration;


    public override void Apply(Monster monster)
    {
        monster.MonsterState.AddState(EDebuffType.Slow, slowValue, slowDuration);
    }
}
