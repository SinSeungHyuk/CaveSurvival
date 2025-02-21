using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ultimate_Kai", menuName = "Scriptable Objects/Player/Ultimate/Kai")]
public class UltimateSkillKai : PlayerUltimateSkillSO
{
    [Header("Skill Data For Kai")]
    [SerializeField] private Color tileColor; // �ٲ���� ���� �÷�
    [SerializeField] private int baseDamage; // �⺻ ������
    [SerializeField] private int range; // ����
    [SerializeField] private Material monsterMaterial; // ���Ϳ��� ���� Ư���� ���׸���


    public Color TileColor => tileColor;
    public int BaseDamage => baseDamage;
    public int Range => range;
    public Material MonsterMaterial => monsterMaterial;


    public override UltimateSkillBehaviour CreateSkill(Player player)
        => new UltimateSkillBehaviourKai(player, this); // �ش� SO ����
}