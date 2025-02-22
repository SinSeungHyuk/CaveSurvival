using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class UltimateSkillBehaviour
{
    protected PlayerUltimateSkillSO skillSO; // 해당 스킬의 SO 데이터
    protected Player player;

    private int skillGauge;
    private int currentGauge;

    public PlayerUltimateSkillSO SkillSO => skillSO;
    public ReactiveProperty<float> GaugeRatio { get; private set; } = new ReactiveProperty<float>();



    public UltimateSkillBehaviour(Player player, PlayerUltimateSkillSO data)
    {
        this.player = player;
        this.skillSO = data;
        skillGauge = data.UltimateSkillData.skillGauge;
        currentGauge = 0;
    }

    public async UniTask UseUltimateSkill()
    {
        if (skillGauge <= currentGauge)
        {
            SetGaugeRatio(-skillGauge);

            SoundEffectManager.Instance.PlaySoundEffect(skillSO.UltimateSkillData.startSoundEffect);
            await GameManager.Instance.VCam.VCamUltimateEffectRoutine();
            
            Apply();
        }
    }

    public void SetGaugeRatio(int value)
    {
        currentGauge = Mathf.Clamp(currentGauge + value, 0, skillGauge);
        GaugeRatio.Value = (float)currentGauge / (float)skillGauge;
    }

    protected void PlaySoundEffect(int index = 0)
    {
        SoundEffectManager.Instance.PlaySoundEffect(skillSO.UltimateSkillData.soundEffects[index]);
    }

    // 상속받아서 각자 스킬 구현
    public abstract void Apply();
    public virtual void Release() { }
}
