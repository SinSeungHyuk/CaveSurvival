using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ultimate_Kai", menuName = "Scriptable Objects/Player/Ultimate/Kai")]
public class UltimateSkillKai : PlayerUltimateSkillSO
{
    [Header("Skill Data For Kai")]
    [SerializeField] private Color tileColor; // 바꿔놓을 맵의 컬러
    [SerializeField] private int baseDamage; // 기본 데미지
    [SerializeField] private int range; // 범위
    [SerializeField] private Material monsterMaterial; // 몬스터에게 입힐 특수한 머테리얼


    public Color TileColor => tileColor;
    public int BaseDamage => baseDamage;
    public int Range => range;
    public Material MonsterMaterial => monsterMaterial;


    public override UltimateSkillBehaviour CreateSkill(Player player)
        => new UltimateSkillBehaviourKai(player, this); // 해당 SO 전달
}