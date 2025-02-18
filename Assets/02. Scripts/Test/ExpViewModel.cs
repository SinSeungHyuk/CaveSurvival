using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpViewModel
{
    // Model
    private readonly PlayerStat playerStat;

    public ReactiveProperty<float> ExpPercentage { get; } = new ReactiveProperty<float>();


    public ExpViewModel(PlayerStat playerStat)
    {
        this.playerStat = playerStat;

        this.playerStat.CurrentExp.Subscribe(exp 
            => SetExpPercentage(exp));

        this.playerStat.CurrentExp
            .Where(exp => exp >= playerStat.Exp)
            .Subscribe(_ => playerStat.LevelUp());
    }

    private void SetExpPercentage(int exp)
    {
        ExpPercentage.Value = (float)exp / (float)playerStat.Exp;
    }
}
