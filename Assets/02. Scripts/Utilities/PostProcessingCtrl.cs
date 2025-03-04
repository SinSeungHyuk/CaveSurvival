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
    private float waitTime = 0.75f; // ���Ʈ ȿ�� �պ��ð�


    private void Awake()
    {
        postProcessing = GetComponent<Volume>();

        // Volume.profile.TryGet<T>(out t) => ����Ʈ ���μ��� �������
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
        // �÷��̾� ü���� 30% �̸� && ��ƾ �ѹ��� ����
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
        // ��ƾ�� ��ž�Ǿ��ٰ� �ٽ� ���۵ɶ��� ����� ó���� �ʱ�ȭ����
        float elapsedTime = 0.0f; // ����ð�
        float startValue = 0.3f;
        targetValue = 0.5f;
        isIncrease = true;

        while (true)
        {
            // vignette ũ���� ���۰� (0.3 or 0.5)
            startValue = vignette.intensity.value;

            while (elapsedTime < waitTime)
            {
                // vignette�� ũ�� ���� Lerp�� ����
                vignette.intensity.value = Mathf.Lerp(startValue, targetValue, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime;

                await UniTask.Yield(cancellationToken: cts.Token);
            }

            // ��ǥ ���� ��ȯ (0.3~0.5 or 0.5~0.3)
            isIncrease = !isIncrease;
            targetValue = isIncrease ? 0.5f : 0.3f;
            elapsedTime = 0.0f;
        }
    }
}
