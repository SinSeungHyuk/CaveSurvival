using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour 
{
    private EPool effectName;


    private void OnEnable()
        => ReleaseEffect().Forget();

    public void InitializeHitEffect(EPool effectName)
        => this.effectName = effectName;

    private async UniTask ReleaseEffect()
    {
        // �ǰ� ����Ʈ : 1�ʵڿ� ��Ȱ��ȭ

        await UniTask.Delay(1000);


        if (this.gameObject.activeSelf)
            ObjectPoolManager.Instance.Release(gameObject, effectName);
    }
}
