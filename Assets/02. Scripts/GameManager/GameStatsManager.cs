using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : Singleton<GameStatsManager>
{
    public GameStats CurrentStats { get; private set; } = new GameStats();


    public void AddWeaponTotalDamage(int damage)
    {
        CurrentStats.AddStat(EStatsType.WeaponTotalDamage,damage);
    }
}
