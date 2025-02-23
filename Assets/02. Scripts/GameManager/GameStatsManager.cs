using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : Singleton<GameStatsManager>
{
    private GameStats currentStats; // 게임의 통계가 들어있는 클래스


    protected override void Awake()
    {
        base.Awake();

        currentStats = new GameStats();
    }


    /// <summary>
    /// 무기별 통계는 매개변수로 무기를 넣어야함 -> 메소드 오버로딩
    /// </summary>
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
