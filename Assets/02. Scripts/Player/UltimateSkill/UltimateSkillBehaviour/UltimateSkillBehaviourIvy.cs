using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkillBehaviourIvy : UltimateSkillBehaviour
{
    private UltimateSkillIvy ultimateSkillData;
    private UltimateField ultimateField;


    public UltimateSkillBehaviourIvy(Player player, UltimateSkillIvy so) : base(player, so)
    {
        ultimateSkillData = so;
    }

    public override void Apply()
    {
        // 일반 클래스는 Instantiate 사용 불가능. AddComponent로 직접 컴포넌트 생성
        var UltimateField = new GameObject("UltimateField");
        ultimateField = UltimateField.AddComponent<UltimateField>();
        ultimateField.InitializeUltimateField(player, ultimateSkillData, ultimateSkillData.UltimateField);

        PlaySoundEffect();

        UltimateShieldRoutine().Forget();
    }

    private async UniTask UltimateShieldRoutine()
    {
        await UniTask.Delay(ultimateSkillData.Duration * 1000, cancellationToken: player.DisableCancellation.Token);

        Release();
    }

    public override void Release()
    {
        ultimateField.DestroyField();
    }
}