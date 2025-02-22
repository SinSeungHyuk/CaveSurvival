using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ultimate_Leo", menuName = "Scriptable Objects/Player/Ultimate/Leo")]
public class UltimateSkillLeo : PlayerUltimateSkillSO
{
    [Header("Skill Data For Leo")]
    [SerializeField] private GameObject ultimateShield;
    [SerializeField] private int duration; // 지속시간
    [SerializeField] private int addtionalSpeed; // 이동속도 보너스 수치


    public GameObject UltimateShield => ultimateShield;
    public int Duration => duration;
    public int AddtionalSpeed => addtionalSpeed;


    public override UltimateSkillBehaviour CreateSkill(Player player)
        => new UltimateSkillBehaviourLeo(player, this); // 해당 SO 전달
}