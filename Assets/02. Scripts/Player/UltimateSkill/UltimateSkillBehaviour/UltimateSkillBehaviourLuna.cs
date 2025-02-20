using Cysharp.Threading.Tasks;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UltimateSkillBehaviourLuna : UltimateSkillBehaviour
{
    private UltimateSkillLuna ultimateSkillData;
    private Player player;
    private Material defaultMaterial;

    public UltimateSkillBehaviourLuna(UltimateSkillLuna so) : base(so)
    {
        ultimateSkillData = so; // 특수 데이터 캐스팅 저장
    }

    public override void Apply(Player player)
    {
        Debug.Log($"궁극기 사용 !!!!!!!!! {ultimateSkillData.AdditionalHpRegen} , {ultimateSkillData.UltimateSkillData.skillName}");

        this.player = player;

        defaultMaterial = player.SpriteRenderer.material;
        player.SpriteRenderer.material = ultimateSkillData.LunaMaterial;

        player.Stat.PlayerStatChanged(EStatType.HpRegen, ultimateSkillData.AdditionalHpRegen);
        player.Stat.PlayerStatChanged(EStatType.Defense, ultimateSkillData.AdditionalDefense);

        UltimateLunaRoutine().Forget();
    }

    private async UniTask UltimateLunaRoutine()
    {
        await UniTask.Delay(10000, cancellationToken: player.DisableCancellation.Token);

        Release();
    }

    public override void Release()
    {
        player.SpriteRenderer.material = defaultMaterial;

        player.Stat.PlayerStatChanged(EStatType.HpRegen, -ultimateSkillData.AdditionalHpRegen);
        player.Stat.PlayerStatChanged(EStatType.Defense, -ultimateSkillData.AdditionalDefense);
    }
}
