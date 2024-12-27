using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;

public class MonsterSpawn : MonoBehaviour
{
    private MonsterSpawnEvent monsterSpawnEvent;
    private List<WaveSpawnParameter> waveSpawnParameterList;
    private WaveSpawnParameter currentWaveSpawnParameter;
    private Vector2 spawnPosition;
    private int waveCount;
    private int waveTimer;

    public int WaveCount => waveCount;
    public int WaveTimer => waveTimer;



    private void Awake()
    {
        monsterSpawnEvent = GetComponent<MonsterSpawnEvent>();
    }
    private void OnEnable()
    {
        monsterSpawnEvent.OnMonsterSpawn += MonsterSpawnEvent_OnMonsterSpawn;
        monsterSpawnEvent.OnWaveStart += MonsterSpawnEvent_OnWaveStart;
        monsterSpawnEvent.OnWaveFinish += MonsterSpawnEvent_OnWaveFinish;
    }
    private void OnDisable()                
    {                                       
        monsterSpawnEvent.OnMonsterSpawn -= MonsterSpawnEvent_OnMonsterSpawn;
        monsterSpawnEvent.OnWaveStart -= MonsterSpawnEvent_OnWaveStart;
        monsterSpawnEvent.OnWaveFinish -= MonsterSpawnEvent_OnWaveFinish;
    }


    private void MonsterSpawnEvent_OnMonsterSpawn(MonsterSpawnEvent @event, MonsterSpawnEventArgs args)
    {
        waveSpawnParameterList = args.stage.WaveSpawnParameter;

        monsterSpawnEvent.CallWaveStart(0); // ù ���̺���� ����
    }

    private void MonsterSpawnEvent_OnWaveStart(MonsterSpawnEvent @event, int waveCnt)
    {
        // ���� ���̺꿡 �ش��ϴ� ���̺� ���� �Ķ���� �޾ƿ���
        waveCount = waveCnt;
        currentWaveSpawnParameter = waveSpawnParameterList[waveCount];

        // ���̺� ���ӽð� : 20+5(wave) ~ 60 ����
        waveTimer = Mathf.Clamp(waveTimer, Settings.waveTimer + (Settings.extraTimePerWave * waveCount), 60);
        Debug.Log(WaveTimer);

        if (currentWaveSpawnParameter.isBossWave == true) return; // �������� ���Ŀ� ����

        WaveMonsterSpawn().Forget(); // UniTask ȣ��
    }

    private void MonsterSpawnEvent_OnWaveFinish(MonsterSpawnEvent @event, int waveCnt)
    {
        Debug.Log($"Wave Finish!!! - {waveCount}");
    }


    #region SPAWN FUNCTION
    private async UniTaskVoid WaveMonsterSpawn()
    {
        try
        {
            // ù 1�� ���
            await UniTask.Delay(1000);

            float elapsedTime = 1f;

            // waveTimer�� ���� �ݺ�
            while (elapsedTime < waveTimer)
            {
                RandomSpawn();

                // 1�� ���
                await UniTask.Delay(TimeSpan.FromSeconds(Settings.spawnInterval));

                elapsedTime += Settings.spawnInterval; // ���� ���ݸ�ŭ �ð� �����ֱ�
            }

            monsterSpawnEvent.CallWaveFinish(waveCount); // ���̺� ����
        }
        catch (OperationCanceledException)
        {
            Debug.Log("WaveMonsterSpawn - Spawn Canceled!!!");
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
        GameObject spawner = ObjectPoolManager.Instance.Get("Spawner", RandomSpawnPosition(), Quaternion.identity);

        await UniTask.Delay(1000); // 1�� ���Ŀ� ����

        ObjectPoolManager.Instance.Release(spawner);

        var monster = ObjectPoolManager.Instance.Get("Monster", spawner.transform.position, Quaternion.identity);
        //monster.GetComponent<Monster>().InitializeEnemy(monsterInfo.monsterDetailsSO, waveCount);
        monster.GetComponent<PhotonView>().RPC("InitializeMonster", RpcTarget.All, monsterInfo.monsterDetailsSO.ID, waveCount);
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