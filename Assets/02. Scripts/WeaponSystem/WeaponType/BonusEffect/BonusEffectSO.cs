using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BonusEffectSO : ScriptableObject
{
    // ���⿡ ���� �߰�ȿ�� (�����, ��Ʈ������ ��)

    public virtual void Start() { }
    public abstract void Apply(Monster monster);
    public virtual void Release() { }
}
