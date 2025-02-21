using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct UltimateSkillData
{
    // 모든 궁극기가 '공통'으로 가지는 데이터

    public string skillName;
    public Sprite skillIcon;
    public int skillGauge; // 스킬 사용에 필요한 게이지
    public List<SoundEffectSO> soundEffects;
}
