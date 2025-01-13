using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class StageInfoView : MonoBehaviour 
{
    // StageInfoView: �ؽ�Ʈ UI�� �����ϴ� �Լ��� ����
    // UI ������Ʈ�� StageInfoController���� ���޵� �����Ϳ� ����Ͽ� �̷����
    // ��Ʈ�ѷ��� ���� �����͸� �̿��Ͽ� UI�� ����

    [SerializeField] private TextMeshProUGUI txtWaveCount;
    [SerializeField] private TextMeshProUGUI txtWaveTimer;


    public void SetTxtWaveCount(int waveCnt)
    {
        txtWaveCount.text = $"���̺� {waveCnt+1}";
    }

    public void SetTxtWaveTimer(float waveTimer)
    {
        txtWaveTimer.color = (waveTimer <= 5) ? Settings.legend : Color.white;

        txtWaveTimer.text = waveTimer.ToString();
    }
}
