using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Projectile_", menuName = "Scriptable Objects/Weapon/Projectile/Projectile")]
public class ProjectileDetailsSO : ScriptableObject
{
    // ����ü �̹���
    public Sprite sprite;

    // ����ü ���߽� ȿ�� (����, ���� ��)
    public ProjectileEffectSO projectileEffect;
    // ����ü�� ���� �߰�ȿ�� (�����, ��Ʈ������ ��)
    public List<BonusEffectSO> bonusEffects;
    // ����ü ��� ����
    public bool isPiercing;
    // ����ü ��� ī��Ʈ
    public int piercingCount;
    // ����ü �ӵ�
    public int projectileSpeed = 10;
}
