using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnEvent : MonoBehaviour
{
    public event Action<MonsterSpawnEvent, MonsterSpawnEventArgs> OnStageStart;
    public event Action<MonsterSpawnEvent> OnWaveStart;
    public event Action<MonsterSpawnEvent> OnWaveFinish;
    public event Action<MonsterSpawnEvent> OnStageFinish;

    public void CallStageStart(Stage stage)
    {
        OnStageStart?.Invoke(this, new MonsterSpawnEventArgs()
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

    public void CallStageFinish()
    {
        OnStageFinish?.Invoke(this);
    }
}

public class MonsterSpawnEventArgs : EventArgs
{
    public Stage stage;
}