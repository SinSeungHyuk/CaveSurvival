using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;


public class MonsterSpawn : MonoBehaviour
{
    private Stage stage;
    private MonsterSpawnEvent monsterSpawnEvent;
    private List<WaveSpawnParameter> waveSpawnParameterList; // �� ���̺꺰 ��������
    private WaveSpawnParameter currentWaveSpawnParameter; // ���� ���̺� ��������
    private Vector2 spawnPosition;
    private int waveCount;
    private int waveTimer;
    private float elapsedTime;
    private bool isStageFinish; // �̹� ���������� �������� (Ŭ�����ϰ� ����ص� Ŭ���� ����)
    private SoundEffectSO waveFinishSound;

    private CancellationTokenSource cts = new CancellationTokenSource();

    public int WaveCount => waveCount;
    public int WaveTimer => waveTimer;
    public float ElapsedTime => elapsedTime;



    private void Awake()
    {
        monsterSpawnEvent = GetComponent<MonsterSpawnEvent>();
        stage = GetComponent<Stage>();
    }
    private void Start()
    {
        waveFinishSound = AddressableManager.Instance.GetResource<SoundEffectSO>("SoundEffect_WaveClear");
    }
    private void OnEnable()
    {
        monsterSpawnEvent.OnStageStart += MonsterSpawnEvent_OnStageStart;
        monsterSpawnEvent.OnWaveStart += MonsterSpawnEvent_OnWaveStart;
        monsterSpawnEvent.OnWaveFinish += MonsterSpawnEvent_OnWaveFinish;
        monsterSpawnEvent.OnStageFinish += MonsterSpawnEvent_OnStageFinish;
    }
    private void OnDisable()                
    {                                       
        monsterSpawnEvent.OnStageStart -= MonsterSpawnEvent_OnStageStart;
        monsterSpawnEvent.OnWaveStart -= MonsterSpawnEvent_OnWaveStart;
        monsterSpawnEvent.OnWaveFinish -= MonsterSpawnEvent_OnWaveFinish;
        monsterSpawnEvent.OnStageFinish -= MonsterSpawnEvent_OnStageFinish;

        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }


    #region STAGE EVENT
    private void MonsterSpawnEvent_OnStageStart(MonsterSpawnEvent @event, MonsterSpawnEventArgs Stage)
    {
        waveCount = 0; // ù ���̺���� ����

        waveSpawnParameterList = Stage.stage.WaveSpawnParameter;

        monsterSpawnEvent.CallWaveStart(); // ù ���̺���� ����
    }

    private void MonsterSpawnEvent_OnWaveStart(MonsterSpawnEvent @event)
    {
        // ���� ���̺꿡 �ش��ϴ� ���̺� ���� �Ķ���� �޾ƿ���
        currentWaveSpawnParameter = waveSpawnParameterList[waveCount];

        // ���̺� ���ӽð� : 20+5(*wave) ~ 60 ����
        waveTimer = UtilitieHelper.GetWaveTimer(waveCount);     

        WaveMonsterSpawn().Forget(); // UniTask ȣ��
    }

    private void MonsterSpawnEvent_OnWaveFinish(MonsterSpawnEvent @event)
    {
        if (waveCount == Settings.lastWave) // ������ ���̺� Ŭ����
        {
            monsterSpawnEvent.CallStageFinish();

            return;
        }

        GameManager.Instance.Player.PlayerWaveBuff.InitializePlayerWaveBuff();
        GameManager.Instance.UIController.WaveFinishController.InitializeWaveFinishController();
        SoundEffectManager.Instance.PlaySoundEffect(waveFinishSound); // ���̺� ���� ȿ����

        waveCount++; // ���̺� ī��Ʈ 1 ������Ű��
    }

    private void MonsterSpawnEvent_OnStageFinish(MonsterSpawnEvent @event)
    {
        SoundEffectManager.Instance.PlaySoundEffect(waveFinishSound); // ���̺� ���� ȿ����

        // ���������� Ŭ���������� �� �������� Ŭ���� �̺�Ʈ�� ȣ����� �ʵ��� ����
        if (isStageFinish)
            return;

        isStageFinish = true;

        StageFinishEffect().Forget();
    }
    #endregion

    private async UniTask StageFinishEffect()
    {
        GameManager.Instance.VCam.VCamStageFinishEffect();
        Time.timeScale = 0.25f;

        // ignoreTimeScale : ������ timeScale�� �������� �̸� �����ϴ� �ɼ� (false ����Ʈ)
        await UniTask.Delay(2500, ignoreTimeScale: true, cancellationToken: cts.Token);

        // �������� ���� �� ���� ����Ͽ� �������ֱ�
        stage.SetReward(waveCount);
        GameManager.Instance.UIController.StageFinishController.InitializeStageFinishController(stage.AchiveReward, stage.GoldReward);
    }


    #region SPAWN FUNCTION
    private async UniTaskVoid WaveMonsterSpawn()
    {
        try
        {
            // ù 1�� ���
            await UniTask.Delay(1000, cancellationToken:cts.Token);

            if (currentWaveSpawnParameter.isBossWave == true) BossSpawn(); // ��������

            elapsedTime = 1f;
            monsterSpawnEvent.CallElapsedTimeChanged(elapsedTime); // ����ð� ���� �̺�Ʈ

            // waveTimer�� ���� �ݺ�
            while (elapsedTime <= waveTimer - 1)
            {
                RandomSpawn();

                // 1�� ���
                await UniTask.Delay(TimeSpan.FromSeconds(Settings.spawnInterval), cancellationToken: cts.Token);

                elapsedTime += Settings.spawnInterval; // ���� ���ݸ�ŭ �ð� �����ֱ�
                monsterSpawnEvent.CallElapsedTimeChanged(elapsedTime); // ����ð� ���� �̺�Ʈ
            }

            monsterSpawnEvent.CallWaveFinish(); // ���̺� ����
        }
        catch (OperationCanceledException)
        {
            //Debug.Log("WaveMonsterSpawn - Spawn Canceled!!!");
        }
    }

    private void BossSpawn()
    {
        foreach (var bossInfo in currentWaveSpawnParameter.spawnableBossList)
        {
            var monster = ObjectPoolManager.Instance.Get(EPool.Boss, RandomSpawnPosition(), Quaternion.identity);
            monster.GetComponent<Monster>().InitializeMonster(bossInfo, waveCount);
        }
    }

    private void RandomSpawn()
    {
        List<MonsterSpawnParameter> monsterParameters = currentWaveSpawnParameter.monsterSpawnParameters;

        // totalRatio : ������ ����Ȯ�� ���� ���� ��
        int totalRatio = monsterParameters.Sum(x => x.Ratio);
        // batchSpawnCount : ���ÿ� ������ ������ ��
        int batchSpawnCount = currentWaveSpawnParameter.batchSpawnCount;

        // �� batchSpawnCount���� ���� ����
        for (int i = 0; i < batchSpawnCount; i++)
        {
            // ����, ���� ������ ����Ȯ�� ������
            int randomNumber = UnityEngine.Random.Range(0, totalRatio);
            int ratioSum = 0;

            foreach (var monsterInfo in monsterParameters)
            {
                // ���� ��ȸ���� ���Ͱ� ������ ���ԵǸ� ������÷
                ratioSum += monsterInfo.Ratio;
                if (randomNumber < ratioSum)
                {
                    Spawn(monsterInfo).Forget();
                    break;
                }
            }
        }
    }

    private async UniTaskVoid Spawn(MonsterSpawnParameter monsterInfo)
    {
        // �����Ǳ� ���� �ٴڿ� ������ ������Ʈ ����ֱ�
        GameObject spawner = ObjectPoolManager.Instance.Get(EPool.Spawner, RandomSpawnPosition(), Quaternion.identity);

        await UniTask.Delay(900, cancellationToken: cts.Token); // 0.9�� ���Ŀ� ����

        ObjectPoolManager.Instance.Release(spawner,EPool.Spawner);

        var monster = ObjectPoolManager.Instance.Get(EPool.Monster, spawner.transform.position, Quaternion.identity);
        monster.GetComponent<Monster>().InitializeMonster(monsterInfo.monsterDetailsSO, waveCount);
    }

    private Vector2 RandomSpawnPosition()
    {
        // new Ű����� ��� �� vector2�� �����ϴ� �ͺ��� ���������� �ΰ� �����ϴ°� ���ٰ� �Ǵ�
        spawnPosition.x = UnityEngine.Random.Range(-Settings.stageBoundary, Settings.stageBoundary); 
        spawnPosition.y = UnityEngine.Random.Range(-Settings.stageBoundary, Settings.stageBoundary);
        return spawnPosition;
    }
    #endregion
}