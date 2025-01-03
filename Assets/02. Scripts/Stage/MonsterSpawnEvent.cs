using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnEvent : MonoBehaviour
{
    public event Action<MonsterSpawnEvent, MonsterSpawnEventArgs> OnMonsterSpawn;
    public event Action<MonsterSpawnEvent> OnWaveStart;
    public event Action<MonsterSpawnEvent> OnWaveFinish;

    public void CallMonsterSpawn(Stage stage)
    {
        OnMonsterSpawn?.Invoke(this, new MonsterSpawnEventArgs()
        {
            stage = stage
        });
    }

    public void CallWaveStart()
    {
        OnWaveStart?.Invoke(this);
    }

    public void CallWaveFinish()
    {
        OnWaveFinish?.Invoke(this);
    }
}

public class MonsterSpawnEventArgs : EventArgs
{
    public Stage stage;
}