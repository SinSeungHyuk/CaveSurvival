using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_", menuName = "Scriptable Objects/Player/Player")]
public class PlayerDetailsSO : IdentifiedObject
{
    public string characterName; // ĳ���� �̸�
    public Player player; // �÷��̾� ������
    public RuntimeAnimatorController runtimeAnimatorController;
    public Sprite playerSprite;
    public WeaponDetailsSO playerStartingWeapon; // ��Ÿ�� ����
    [TextArea] public string characterStrength; // ĳ���� �Ұ��� ���� �ؽ�Ʈ
    [TextArea] public string characterWeakness;

    [Space(10)]
    [Header("Character Stat")]
    public float Hp;
    public float HpRegen;
    //public float HpSteal;
    public int Defense;
    public int BonusDamage; 
    public int MeleeDamage;
    public int RangeDamage;
    public float Speed;
    public int Dodge;
    public float PickUpRange;
    public int ExpBonus;

}
