using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public event Action OnWaveFinished;

    public Stage CurrentStage {  get; private set; }


    public void CreateStage(StageDetailsSO stageDetails) // �������� ����
    {
        Instantiate(stageDetails.stagePrefab, this.transform); // Instantiate���� this.transform �̹Ƿ� �ڽ����� ������
        Stage instantiatedStage = GetComponentInChildren<Stage>(); // ������ �ڽĿ�����Ʈ���� Stage ������Ʈ ��������
        instantiatedStage.InitializedStage(stageDetails); // ������ �������� �ʱ�ȭ

        CurrentStage = instantiatedStage;
    }

    public void CallWaveFinished() 
        => OnWaveFinished?.Invoke();

    public void CallNextWaveStart()
        => CurrentStage.MonsterSpawnEvent.CallWaveStart();
}
