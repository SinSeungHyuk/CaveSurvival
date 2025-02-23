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
        // �Ϲ� Ŭ������ Instantiate ��� �Ұ���. AddComponent�� ���� ������Ʈ ����
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