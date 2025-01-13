using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoController : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private StageInfoView stageInfoView;
    
    // Model 역할을 하게 될 데이터들
    private Stage currentStage;
    private int waveTimer;


    public void InitializeStageInfoController()
    {
        // Model 역할을 하는 웨이브의 정보가 변경되는 이벤트에 함수 구독
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
        // 웨이브가 새로 시작하면 현재 웨이브 카운트 보여주기
        stageInfoView.SetTxtWaveCount(currentStage.MonsterSpawner.WaveCount);
        
        waveTimer = currentStage.MonsterSpawner.WaveTimer;
        stageInfoView.SetTxtWaveTimer(waveTimer);
    }

    private void MonsterSpawnEvent_OnElapsedTimeChanged(MonsterSpawnEvent @event, float elapsedTime)
    {
        // 현재 경과한 시간만큼 빼서 남은 시간 보여주기
        stageInfoView.SetTxtWaveTimer(waveTimer - elapsedTime);
    }
}
