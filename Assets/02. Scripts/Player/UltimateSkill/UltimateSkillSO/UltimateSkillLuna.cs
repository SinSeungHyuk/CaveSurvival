using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ultimate_Luna", menuName = "Scriptable Objects/Player/Ultimate/Luna")]
public class UltimateSkillLuna : PlayerUltimateSkillSO
{
    [Header("Skill Data For Luna")]
    [SerializeField] private int additionalHpRegen; // 체력재생 보너스 스탯
    [SerializeField] private int additionalDefense; // 방어력 보너스 스탯
    [SerializeField] private Material lunaMaterial; // 특수한 머테리얼

    
    public int AdditionalHpRegen => additionalHpRegen;
    public int AdditionalDefense => additionalDefense;
    public Material LunaMaterial => lunaMaterial;


    public override UltimateSkillBehaviour CreateSkill(Player player)
        => new UltimateSkillBehaviourLuna(player, this); // 해당 SO 전달
}
