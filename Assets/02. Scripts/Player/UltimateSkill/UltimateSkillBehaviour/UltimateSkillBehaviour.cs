using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class UltimateSkillBehaviour
{
    protected PlayerUltimateSkillSO data; // �ش� ��ų�� SO ������

    public UltimateSkillBehaviour(PlayerUltimateSkillSO data)
    {
        this.data = data;
    }

    // ��ӹ޾Ƽ� ���� ��ų ����
    public abstract void Apply(Player player);
    public virtual void Release() { }
}
