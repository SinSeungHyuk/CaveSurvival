using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelViewModel
{
    // Model
    private readonly PlayerStat playerStat;

    public ReactiveProperty<string> PlayerLevel { get; } = new ReactiveProperty<string>();


    public PlayerLevelViewModel(PlayerStat playerStat)
    {
        this.playerStat = playerStat;

        this.playerStat.Level.Subscribe(level
            => SetPlayerLevel(level));
    }

    private void SetPlayerLevel(int level)
    {
        PlayerLevel.Value = "Level. " + level;
    }
}