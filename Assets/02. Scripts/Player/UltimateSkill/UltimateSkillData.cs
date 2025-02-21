using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct UltimateSkillData
{
    // ��� �ñرⰡ '����'���� ������ ������

    public string skillName;
    public Sprite skillIcon;
    public int skillGauge; // ��ų ��뿡 �ʿ��� ������
    public List<SoundEffectSO> soundEffects;
}
