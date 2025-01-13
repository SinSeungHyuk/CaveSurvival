using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoController : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private StageInfoView stageInfoView;
    
    // Model ������ �ϰ� �� �����͵�
    private Stage currentStage;
    private int waveTimer;


    public void InitializeStageInfoController()
    {
        // Model ������ �ϴ� ���̺��� ������ ����Ǵ� �̺�Ʈ�� �Լ� ����
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveStart += MonsterSpawnEvent_OnWaveStart;
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnElapsedTimeChanged += MonsterSpawnEvent_OnElapsedTimeChanged;
        
        currentStage = StageManager.Instance.CurrentStage;
        waveTimer = currentStage.MonsterSpawner.WaveTimer;
    }

    private void OnDisable()
    {
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveStart -= MonsterSpawnEvent_OnWaveStart;
    }


    private void MonsterSpawnEvent_OnWaveStart(MonsterSpawnEvent @event)
    {
        // ���̺갡 ���� �����ϸ� ���� ���̺� ī��Ʈ �����ֱ�
        stageInfoView.SetTxtWaveCount(currentStage.MonsterSpawner.WaveCount);
        
        waveTimer = currentStage.MonsterSpawner.WaveTimer;
        stageInfoView.SetTxtWaveTimer(waveTimer);
    }

    private void MonsterSpawnEvent_OnElapsedTimeChanged(MonsterSpawnEvent @event, float elapsedTime)
    {
        // ���� ����� �ð���ŭ ���� ���� �ð� �����ֱ�
        stageInfoView.SetTxtWaveTimer(waveTimer - elapsedTime);
    }
}
