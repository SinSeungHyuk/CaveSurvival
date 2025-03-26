using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ProjectileData
{
    // ����ü �̹���
    public Sprite sprite;
    // ����ü ���׸���
    public Material material;

    // ����ü ���߽� ȿ�� (����, ���� ��)
    public ProjectileEffectSO projectileEffect;
    // ����ü ��� ����
    public bool isPiercing;
    // ����ü ��� ī��Ʈ
    public int piercingCount;
    // ����ü �ӵ�
    public int projectileSpeed;
}
