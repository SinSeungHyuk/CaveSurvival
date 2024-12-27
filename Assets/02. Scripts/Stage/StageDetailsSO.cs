using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage_", menuName = "Scriptable Objects/Stage/Stage")]
public class StageDetailsSO : ScriptableObject
{
    public GameObject stagePrefab;

    public string roomName;
    //public MusicTrackSO roomMusic; // �������� ����
    // ���� ũ��� �����ϴٴ� �����Ͽ� ����

    public List<WaveSpawnParameter> waveSpawnParameter;
}

[Serializable]
public struct WaveSpawnParameter
{
    public int batchSpawnCount; // �ѹ��� ������ ������ ��
    public List<MonsterSpawnParameter> monsterSpawnParameters; // ������ ���� ����&Ȯ��

    public bool isBossWave;
    public List<MonsterDetailsSO> spawnableBossList; // �������̺��� ��� ������ ���� ����Ʈ
}

[Serializable] // �� ���̺긶�� ������ ���� �Է�
public struct MonsterSpawnParameter
{
    public MonsterDetailsSO monsterDetailsSO; // ������ ����
    public int Ratio; // ���Ͱ� ������ Ȯ��
}