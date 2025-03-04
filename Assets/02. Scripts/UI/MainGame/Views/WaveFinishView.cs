using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Text;

public class WaveFinishView : MonoBehaviour
{
    [SerializeField] private Image imgBackground;
    [SerializeField] private TextMeshProUGUI txtWaveFinish;
    [SerializeField] private BtnWaveFinishUI btnWaveFinish;

    private string text = "웨이브 완료!";


    public void InitializeWaveFinishView()
    {
        txtWaveFinish.text = "";
        imgBackground.gameObject.SetActive(true);

        imgBackground.DOFade(0.7f, 2f)
            .OnComplete(() => TypeTextEffect().Forget());
    }

    private async UniTask TypeTextEffect()
    {
        txtWaveFinish.gameObject.SetActive(true);
        StringBuilder sb = new StringBuilder();

        foreach (var ch in text)
        {
            sb.Append(ch);
            txtWaveFinish.text = sb.ToString();

            await UniTask.Delay(200);
        }

        await UniTask.Delay(500);

        btnWaveFinish.BtnConfirm.onClick.RemoveAllListeners();
        btnWaveFinish.BtnConfirm.onClick.AddListener(CallNextWaveStart);
        btnWaveFinish.InitializeBtnWaveFinish();

        await UniTask.Yield();

        btnWaveFinish.gameObject.SetActive(true);
    }

    private void CallNextWaveStart()
    {
        // UI가 사라지면서 다시 투명하게 수정 (재활용하기 위해)
        Color transparent = imgBackground.color;
        transparent.a = 0f; // 투명
        imgBackground.color = transparent;

        imgBackground.gameObject.SetActive(false);
        btnWaveFinish.gameObject.SetActive(false);

        StageManager.Instance.CurrentStage.MonsterSpawnEvent.CallWaveStart();
    }
}
