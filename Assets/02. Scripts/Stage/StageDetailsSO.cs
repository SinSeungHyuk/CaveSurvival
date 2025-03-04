using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage_", menuName = "Scriptable Objects/Stage/Stage")]
public class StageDetailsSO : IdentifiedObject
{
    public GameObject stagePrefab;

    public string roomName;
    public EStageType stageType;
    [TextArea] public string roomDescription;
    public List<MusicTrackSO> roomMusic; // 스테이지 음악
    // 맵의 크기는 동일하다는 가정하에 제작

    public List<WaveSpawnParameter> waveSpawnParameter;

    public Sprite bossSprite; // 이 스테이지의 보스 이미지

    [Space(10)]
    [Header("Reward")]
    public int achive;
    public int gold;

    [Space(10)]
    [Header("Unlock")]
    public bool isUnlock; // 해금 여부
    public int unlockCost; // 해금 비용
}

[Serializable]
public struct WaveSpawnParameter
{
    public int batchSpawnCount; // 한번에 스폰할 몬스터의 수
    public List<MonsterSpawnParameter> monsterSpawnParameters; // 스폰할 몬스터 종류&확률

    public bool isBossWave;
    public List<MonsterDetailsSO> spawnableBossList; // 보스웨이브일 경우 스폰될 보스 리스트
}

[Serializable] // 각 웨이브마다 몬스터의 정보 입력
public struct MonsterSpawnParameter
{
    public MonsterDetailsSO monsterDetailsSO; // 몬스터의 정보
    public int Ratio; // 몬스터가 스폰될 확률
}