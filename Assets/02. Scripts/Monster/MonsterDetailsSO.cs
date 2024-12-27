using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterDetails_", menuName = "Scriptable Objects/Monster/Monster Details")]
public class MonsterDetailsSO : IdentifiedObject
{
    [Header("Base Monster Details")]
    public string enemyName;
    public Sprite sprite; // ���� ��������Ʈ
    public Color spriteColor; // ��������Ʈ ����
    public RuntimeAnimatorController runtimeAnimatorController; // ���� �ִϸ�����

    [Header("Base Monster Stats")]
    public MonsterMovementSO movementType; // �̵�Ÿ�� (����, ����, �̵�)
    public MonsterAttackSO attackType; // ����Ÿ�� (����ü, ���� ��)
    public float speed = 3f;
    public float chaseDistance = 50f; // �÷��̾���� �ִ� ����
    public float baseDamage = 10; // �÷��̾� ���� ������ ���� ��� ������ (����ü,����)
    public float maxHp = 10; // �ִ�ü��
    public float waveBonusHp = 1; // ���̺긶�� �����ϴ� �ִ�ü��
    public float waveBonusDmg = 1; // ���̺긶�� �����ϴ� ���ݷ�

    [Header("ETC")]
    public ItemDetailsSO itemDetails;
    public Material enemyStandardMaterial;
    public Material enemyHitMaterial;
    //public float enemyMaterializeTime;
    //public Shader enemyMaterializeShader;
    //public Color enemyMaterializeColor;
}
