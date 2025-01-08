using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : Singleton<GameStatsManager>
{
    private GameStats currentStats;

    protected override void Awake()
    {
        base.Awake();

        currentStats = new GameStats();
    }


    public void AddStats(EStatsType type)
    {
        if (type == EStatsType.PlayerTotalKills)
            currentStats.AddPlayerTotalKills();
    }
    public void AddStats(WeaponDetailsSO weapon ,EStatsType type, int value)
        => currentStats.AddWeaponStats(weapon, type, value);

    public int GetStats(WeaponDetailsSO weapon, EStatsType type)
    {
        if (currentStats.WeaponStats.TryGetValue(weapon, out var statsData)) {
            if (statsData.TryGetValue(type, out var stats))
                return stats;
        }

        return 0;
    }
    public int GetStats(EStatsType type)
    {
        if (type == EStatsType.PlayerTotalKills)
            return currentStats.PlayerTotalKills;

        return -1;
    }
}
