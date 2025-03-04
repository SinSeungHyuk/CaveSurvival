using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingCtrl : MonoBehaviour
{
    private Volume postProcessing;
    private Vignette vignette;
    private Player player;

    // Vignette Property
    private CancellationTokenSource cts;
    private bool isVignetteRoutine = false;
    private bool isIncrease = true;
    private float targetValue = 0.5f;
    private float waitTime = 0.75f; // 비네트 효과 왕복시간


    private void Awake()
    {
        postProcessing = GetComponent<Volume>();

        // Volume.profile.TryGet<T>(out t) => 포스트 프로세싱 볼륨얻기
        postProcessing.profile.TryGet<Vignette>(out vignette);
    }

    public void InitializePostProcessingCtrl(Player player)
    {
        this.player = player;
        player.Stat.OnHpChanged += Stat_OnHpChanged;

        vignette.active = false;
    }
    private void OnDisable()
    {
        player.Stat.OnHpChanged -= Stat_OnHpChanged;
        cts?.Cancel();
        cts?.Dispose();
    }


    private void Stat_OnHpChanged(PlayerStat @event, float hp)
    {
        // 플레이어 체력이 30% 미만 && 루틴 한번만 실행
        if (player.Stat.GetHPStatRatio() <= 0.3f && !isVignetteRoutine)
        {
            cts?.Dispose();
            cts = new CancellationTokenSource();
            isVignetteRoutine = true;
            vignette.active = true;
            vignetteRoutine().Forget();
        }

        else if (player.Stat.GetHPStatRatio() > 0.3f)
        {
            cts?.Cancel();
            vignette.active = false;
            isVignetteRoutine = false;
        }
    }

    private async UniTask vignetteRoutine()
    {
        // 루틴이 스탑되었다가 다시 시작될때를 대비해 처음에 초기화해줌
        float elapsedTime = 0.0f; // 경과시간
        float startValue = 0.3f;
        targetValue = 0.5f;
        isIncrease = true;

        while (true)
        {
            // vignette 크기의 시작값 (0.3 or 0.5)
            startValue = vignette.intensity.value;

            while (elapsedTime < waitTime)
            {
                // vignette의 크기 값을 Lerp로 보간
                vignette.intensity.value = Mathf.Lerp(startValue, targetValue, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime;

                await UniTask.Yield(cancellationToken: cts.Token);
            }

            // 목표 값을 전환 (0.3~0.5 or 0.5~0.3)
            isIncrease = !isIncrease;
            targetValue = isIncrease ? 0.5f : 0.3f;
            elapsedTime = 0.0f;
        }
    }
}
