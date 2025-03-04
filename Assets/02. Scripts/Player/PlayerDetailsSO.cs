using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_", menuName = "Scriptable Objects/Player/Player")]
public class PlayerDetailsSO : IdentifiedObject
{
    public string characterName; // 캐릭터 이름
    public Player player; // 플레이어 프리팹
    public RuntimeAnimatorController runtimeAnimatorController;
    public Sprite playerSprite;
    public WeaponDetailsSO playerStartingWeapon; // 스타팅 무기
    public PlayerUltimateSkillSO playerStartingUltimateSkill; // 스타팅 궁극기
    [TextArea] public string characterStrength; // 캐릭터 소개를 위한 텍스트
    [TextArea] public string characterWeakness;

    [Space(10)]
    [Header("Character Stat")]
    public float Hp;
    public int HpRegen;
    public int Defense;
    public int BonusDamage; 
    public int MeleeDamage;
    public int RangeDamage;
    public float Speed;
    public int Dodge;
    public int PickUpRange;
    public int ExpBonus;

    [Space(10)]
    [Header("Unlock")]
    public bool isUnlock; // 해금 여부
    public int unlockCost; // 해금 비용
}
