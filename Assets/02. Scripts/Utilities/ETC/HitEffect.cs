using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HitEffect : MonoBehaviour 
{
    private EPool effectName;
    private CancellationTokenSource cts;


    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    public void InitializeHitEffect(EPool effectName)
    {
        cts = new CancellationTokenSource();
        this.effectName = effectName;

        ReleaseEffect().Forget();
    }
    

    private async UniTask ReleaseEffect()
    {
        // 피격 이펙트 : 1초뒤에 비활성화

        await UniTask.Delay(1000, cancellationToken:cts.Token);


        if (this.gameObject.activeSelf)
            ObjectPoolManager.Instance.Release(gameObject, effectName);
    }
}
