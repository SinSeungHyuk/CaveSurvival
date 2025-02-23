using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public Stage CurrentStage {  get; private set; }


    public void CreateStage(StageDetailsSO stageDetails) // �������� ����
    {
        var stage = Instantiate(stageDetails.stagePrefab, this.transform); // Instantiate���� this.transform �̹Ƿ� �ڽ����� ������
        CurrentStage = stage.GetComponent<Stage>(); // ���� �������� ������Ƽ ����
        CurrentStage.InitializedStage(stageDetails); // �������� �ʱ�ȭ ����
    }
}
