using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ultimate_Luna", menuName = "Scriptable Objects/Player/Ultimate/Luna")]
public class UltimateSkillLuna : PlayerUltimateSkillSO
{
    [Header("Skill Data For Luna")]
    [SerializeField] private int additionalHpRegen; // ü����� ���ʽ� ����
    [SerializeField] private int additionalDefense; // ���� ���ʽ� ����
    [SerializeField] private Material lunaMaterial; // Ư���� ���׸���

    
    public int AdditionalHpRegen => additionalHpRegen;
    public int AdditionalDefense => additionalDefense;
    public Material LunaMaterial => lunaMaterial;


    public override UltimateSkillBehaviour CreateSkill(Player player)
        => new UltimateSkillBehaviourLuna(player, this); // �ش� SO ����
}
