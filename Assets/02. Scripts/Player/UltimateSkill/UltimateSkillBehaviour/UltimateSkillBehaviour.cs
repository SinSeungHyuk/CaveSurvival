using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class UltimateSkillBehaviour
{
    protected PlayerUltimateSkillSO data; // 해당 스킬의 SO 데이터

    public UltimateSkillBehaviour(PlayerUltimateSkillSO data)
    {
        this.data = data;
    }

    // 상속받아서 각자 스킬 구현
    public abstract void Apply(Player player);
    public virtual void Release() { }
}
