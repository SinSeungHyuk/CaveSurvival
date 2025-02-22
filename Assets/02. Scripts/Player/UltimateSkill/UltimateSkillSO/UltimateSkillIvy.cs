using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ultimate_Ivy", menuName = "Scriptable Objects/Player/Ultimate/Ivy")]
public class UltimateSkillIvy : PlayerUltimateSkillSO
{
    [Header("Skill Data For Ivy")]
    [SerializeField] private GameObject ultimateField;
    [SerializeField] private int duration; // 지속시간
    [SerializeField] private int addtionalDodge; // 회피율 보너스 수치


    public GameObject UltimateField => ultimateField;
    public int Duration => duration;
    public int AddtionalDodge => addtionalDodge;


    public override UltimateSkillBehaviour CreateSkill(Player player)
        => new UltimateSkillBehaviourIvy(player, this); // 해당 SO 전달
}