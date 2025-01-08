using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    /// <summary>
    /// 1. 무기별 딜량
    /// 2. 무기별 DPS (무기별 들고있던 시간)
    /// 3. 플레이어의 킬수
    /// </summary>

    // 각 무기별 <통계타입, 값>
    private Dictionary<WeaponDetailsSO, Dictionary<EStatsType, int>> weaponStats = new Dictionary<WeaponDetailsSO, Dictionary<EStatsType, int>>();
    private int playerTotalKills;

    public IReadOnlyDictionary<WeaponDetailsSO, Dictionary<EStatsType, int>> WeaponStats => weaponStats;
    public int PlayerTotalKills => playerTotalKills;


    public void AddWeaponStats(WeaponDetailsSO weapon, EStatsType type, int value)
    {
        // 만약 통계 데이터에 없는 무기라면
        if (weaponStats.TryGetValue(weapon, out var statsData) == false)
        {
            // 새 무기를 추가
            statsData = new Dictionary<EStatsType, int>();
            weaponStats[weapon] = statsData;
        }

        // 해당 무기의 통계누적이 최초인지 아닌지
        if (!statsData.TryGetValue(type, out _))
            statsData[type] = value; // 초기화
        else
            statsData[type] += value; // 기존 값에 누적
    }

    public void AddPlayerTotalKills()
        => playerTotalKills++;

    public void ResetStats()
    {
        playerTotalKills = 0;
        weaponStats.Clear();
    }
}
