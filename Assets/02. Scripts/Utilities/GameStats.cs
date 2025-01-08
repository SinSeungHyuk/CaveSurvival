using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    /// <summary>
    /// 1. ���⺰ ����
    /// 2. ���⺰ DPS (���⺰ ����ִ� �ð�)
    /// 3. �÷��̾��� ų��
    /// </summary>

    // �� ���⺰ <���Ÿ��, ��>
    private Dictionary<WeaponDetailsSO, Dictionary<EStatsType, int>> weaponStats = new Dictionary<WeaponDetailsSO, Dictionary<EStatsType, int>>();
    private int playerTotalKills;

    public IReadOnlyDictionary<WeaponDetailsSO, Dictionary<EStatsType, int>> WeaponStats => weaponStats;
    public int PlayerTotalKills => playerTotalKills;


    public void AddWeaponStats(WeaponDetailsSO weapon, EStatsType type, int value)
    {
        // ���� ��� �����Ϳ� ���� ������
        if (weaponStats.TryGetValue(weapon, out var statsData) == false)
        {
            // �� ���⸦ �߰�
            statsData = new Dictionary<EStatsType, int>();
            weaponStats[weapon] = statsData;
        }

        // �ش� ������ ��贩���� �������� �ƴ���
        if (!statsData.TryGetValue(type, out _))
            statsData[type] = value; // �ʱ�ȭ
        else
            statsData[type] += value; // ���� ���� ����
    }

    public void AddPlayerTotalKills()
        => playerTotalKills++;

    public void ResetStats()
    {
        playerTotalKills = 0;
        weaponStats.Clear();
    }
}
