using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnEvent : MonoBehaviour
{
    public event Action<MonsterSpawnEvent, MonsterSpawnEventArgs> OnMonsterSpawn;
    public event Action<MonsterSpawnEvent, int> OnWaveStart; // ������ ���̺��� ��ȣ (�� ���̺�����)
    public event Action<MonsterSpawnEvent, int> OnWaveFinish;

    public void CallMonsterSpawn(Stage stage)
    {
        OnMonsterSpawn?.Invoke(this, new MonsterSpawnEventArgs()
        {
            stage = stage
        });
    }

    public void CallWaveStart(int waveCnt)
    {
        OnWaveStart?.Invoke(this, waveCnt);
    }

    public void CallWaveFinish(int waveCnt)
    {
        OnWaveFinish?.Invoke(this, waveCnt);
    }
}

public class MonsterSpawnEventArgs : EventArgs
{
    public Stage stage;
}