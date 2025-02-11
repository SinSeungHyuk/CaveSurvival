using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public MonsterSpawnEvent MonsterSpawnEvent { get; private set; }
    public MonsterSpawn MonsterSpawner { get; private set; }
    public StageDetailsSO StageDetails { get; private set; } // ���� ������ ��� SO
    public List<WaveSpawnParameter> WaveSpawnParameter { get; private set; } // ���̺꺰 ��������
    public int AchiveReward {  get; private set; } 
    public int GoldReward {  get; private set; } // ��庸��


    private void Awake()
    {
        MonsterSpawnEvent = GetComponent<MonsterSpawnEvent>();
        MonsterSpawner = GetComponent<MonsterSpawn>();
    }

    public void InitializedStage(StageDetailsSO stageDetails)
    {
        StageDetails = stageDetails;
        WaveSpawnParameter = stageDetails.waveSpawnParameter;

        MonsterSpawnEvent.CallStageStart(this); // �� �ʱ�ȭ�� ��� ����� �Ŀ� ���� �̺�Ʈ ȣ��

        // �������� ���� �������
        MusicManager.Instance.PlayMusic(stageDetails.roomMusic[Random.Range(0, stageDetails.roomMusic.Count)]);
    }

    public void SetReward(int waveCount)
    {
        // ���������� ����� �������� ���� ���
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
        // ���̺꺰 �� ���������� �⺻ ���� ���
        float waveRatio = (float)wave / Settings.lastWave;
        return (int)(totalReward * Mathf.Pow(waveRatio, Settings.rewardScaling));
    }

    private int SetKillBonusReward(float totalReward, int wave)
    {
        // �÷��̾��� ų���� ���� ���� �߰�����
        int kills = GameStatsManager.Instance.GetStats(EStatsType.PlayerTotalKills);

        float waveBonusScale = Mathf.Lerp(1f, 0.1f, (float)wave / Settings.lastWave); // �ʹ� ���̺�� ���� ����ġ
        float bonus = kills * waveBonusScale;
        return (int)Mathf.Min(0.1f * totalReward, bonus); // �ִ� 10% ����
    }
}
