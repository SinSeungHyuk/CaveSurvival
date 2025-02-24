using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;

public class UltimateSkillBehaviourLeo : UltimateSkillBehaviour
{
    private UltimateSkillLeo ultimateSkillData;
    private UltimateShield ultimateShield;


    public UltimateSkillBehaviourLeo(Player player, UltimateSkillLeo so) : base(player, so)
    {
        ultimateSkillData = so;
    }

    public override void Apply()
    {
        // 일반 클래스는 Instantiate 사용 불가능. AddComponent로 직접 컴포넌트 생성

        var UltimateShield = new GameObject("UltimateShield");
        UltimateShield.transform.parent = player.transform;
        ultimateShield = UltimateShield.AddComponent<UltimateShield>();
        ultimateShield.InitializeUltimateShield(player, ultimateSkillData.UltimateShield);

        player.Hitbox.enabled = false;
        player.Stat.PlayerStatChanged(EStatType.SpeedBonus, ultimateSkillData.AddtionalSpeed);

        PlaySoundEffect();
        SetUsingUltimateSkill(true);

        UltimateShieldRoutine().Forget();
    }

    private async UniTask UltimateShieldRoutine()
    {
        await UniTask.Delay(ultimateSkillData.Duration * 1000, cancellationToken: player.DisableCancellation.Token);

        Release();
    }

    public override void Release()
    {
        SetUsingUltimateSkill(false);

        ultimateShield.DestroyShield();

        player.Hitbox.enabled = true;
        player.Stat.PlayerStatChanged(EStatType.SpeedBonus, -ultimateSkillData.AddtionalSpeed);
    }
}