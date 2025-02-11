using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public MonsterSpawnEvent MonsterSpawnEvent { get; private set; }
    public MonsterSpawn MonsterSpawner { get; private set; }
    public StageDetailsSO StageDetails { get; private set; } // 방의 정보가 담긴 SO
    public List<WaveSpawnParameter> WaveSpawnParameter { get; private set; } // 웨이브별 스폰정보
    public int AchiveReward {  get; private set; } 
    public int GoldReward {  get; private set; } // 골드보상


    private void Awake()
    {
        MonsterSpawnEvent = GetComponent<MonsterSpawnEvent>();
        MonsterSpawner = GetComponent<MonsterSpawn>();
    }

    public void InitializedStage(StageDetailsSO stageDetails)
    {
        StageDetails = stageDetails;
        WaveSpawnParameter = stageDetails.waveSpawnParameter;

        MonsterSpawnEvent.CallStageStart(this); // 방 초기화가 모두 진행된 후에 스폰 이벤트 호출

        // 스테이지 음악 랜덤재생
        MusicManager.Instance.PlayMusic(stageDetails.roomMusic[Random.Range(0, stageDetails.roomMusic.Count)]);
    }

    public void SetReward(int waveCount)
    {
        // 스테이지가 종료된 시점에서 보상 계산
        // 
        AchiveReward = SetBaseReward(StageDetails.achive, waveCount);
        AchiveReward += SetKillBonusReward(AchiveReward, waveCount);

        GoldReward = SetBaseReward(StageDetails.gold, waveCount);
        GoldReward += SetKillBonusReward(GoldReward, waveCount);

        RewardDataSO rewardData = AddressableManager.Instance.GetResource<RewardDataSO>("RewardData");
        rewardData.achiveReward = AchiveReward;
        rewardData.goldReward = GoldReward;
    }

    private int SetBaseReward(float totalReward, int wave)
    {
        // 웨이브별 이 스테이지의 기본 보상 계산
        float waveRatio = (float)wave / Settings.lastWave;
        return (int)(totalReward * Mathf.Pow(waveRatio, Settings.rewardScaling));
    }

    private int SetKillBonusReward(float totalReward, int wave)
    {
        // 플레이어의 킬수에 따른 보상 추가지급
        int kills = GameStatsManager.Instance.GetStats(EStatsType.PlayerTotalKills);

        float waveBonusScale = Mathf.Lerp(1f, 0.1f, (float)wave / Settings.lastWave); // 초반 웨이브는 높은 가중치
        float bonus = kills * waveBonusScale;
        return (int)Mathf.Min(0.1f * totalReward, bonus); // 최대 10% 제한
    }
}
