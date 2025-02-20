using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerUltimateSkillSO : ScriptableObject
{
    [SerializeField] private UltimateSkillData ultimateSkillData;

    public UltimateSkillData UltimateSkillData => ultimateSkillData;


    public abstract UltimateSkillBehaviour CreateSkill();
}
