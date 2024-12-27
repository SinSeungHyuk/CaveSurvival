using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public MonsterSpawnEvent MonsterSpawnEvent { get; private set; }
    public MonsterSpawn MonsterSpawner { get; private set; }
    public StageDetailsSO StageDetails { get; private set; } // ���� ������ ��� SO
    public List<WaveSpawnParameter> WaveSpawnParameter { get; private set; } // ���̺꺰 ��������


    private void Awake()
    {
        MonsterSpawnEvent = GetComponent<MonsterSpawnEvent>();
        MonsterSpawner = GetComponent<MonsterSpawn>();
    }

    public void InitializedStage(StageDetailsSO stageDetails)
    {
        StageDetails = stageDetails;
        WaveSpawnParameter = stageDetails.waveSpawnParameter;

        Debug.Log("Stage Init !!!!!!!!!!!!!!!! - is master??? : " + PhotonNetwork.IsMasterClient);

        MonsterSpawnEvent.CallMonsterSpawn(this); // �� �ʱ�ȭ�� ��� ����� �Ŀ� ���� �̺�Ʈ ȣ��
    }
}
