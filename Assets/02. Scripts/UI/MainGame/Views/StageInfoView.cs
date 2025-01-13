using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class StageInfoView : MonoBehaviour 
{
    // StageInfoView: 텍스트 UI를 변경하는 함수만 존재
    // UI 업데이트는 StageInfoController에서 전달된 데이터에 기반하여 이루어짐
    // 컨트롤러와 모델의 데이터를 이용하여 UI를 갱신

    [SerializeField] private TextMeshProUGUI txtWaveCount;
    [SerializeField] private TextMeshProUGUI txtWaveTimer;


    public void SetTxtWaveCount(int waveCnt)
    {
        txtWaveCount.text = $"웨이브 {waveCnt+1}";
    }

    public void SetTxtWaveTimer(float waveTimer)
    {
        txtWaveTimer.color = (waveTimer <= 5) ? Settings.legend : Color.white;

        txtWaveTimer.text = waveTimer.ToString();
    }
}
