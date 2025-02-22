using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ultimate_Leo", menuName = "Scriptable Objects/Player/Ultimate/Leo")]
public class UltimateSkillLeo : PlayerUltimateSkillSO
{
    [Header("Skill Data For Leo")]
    [SerializeField] private GameObject ultimateShield;
    [SerializeField] private int duration; // ���ӽð�
    [SerializeField] private int addtionalSpeed; // �̵��ӵ� ���ʽ� ��ġ


    public GameObject UltimateShield => ultimateShield;
    public int Duration => duration;
    public int AddtionalSpeed => addtionalSpeed;


    public override UltimateSkillBehaviour CreateSkill(Player player)
        => new UltimateSkillBehaviourLeo(player, this); // �ش� SO ����
}