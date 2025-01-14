using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

public class LoadingTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLoading;

    private CancellationTokenSource cts;
    private StringBuilder sb;
    private string baseText = "Loading";
    private int dotCount = 0;


    private void OnEnable()
    {
        cts = new CancellationTokenSource();

        SetTxtLoading().Forget();
    }
    private void OnDisable()
    {
        cts.Cancel();
        cts.Dispose();
    }


    private async UniTask SetTxtLoading()
    {
        while (true)
        {
            sb = new StringBuilder();
            sb.Append(baseText);

            for (int i = 0; i < dotCount; i++)
                sb.Append(" . ");

            txtLoading.text = sb.ToString();

            dotCount = (dotCount + 1) % 4; // 0~3

            await UniTask.Delay(500, cancellationToken: cts.Token);
        }
    }
}
