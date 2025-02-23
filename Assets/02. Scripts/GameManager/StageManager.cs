using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public Stage CurrentStage {  get; private set; }


    public void CreateStage(StageDetailsSO stageDetails) // 스테이지 생성
    {
        var stage = Instantiate(stageDetails.stagePrefab, this.transform); // Instantiate에서 this.transform 이므로 자식으로 생성됨
        CurrentStage = stage.GetComponent<Stage>(); // 현재 스테이지 프로퍼티 설정
        CurrentStage.InitializedStage(stageDetails); // 스테이지 초기화 진행
    }
}
