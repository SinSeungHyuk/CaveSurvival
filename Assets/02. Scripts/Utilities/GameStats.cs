using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    private Dictionary<EStatsType, int> stats = new Dictionary<EStatsType, int>();

    public IReadOnlyDictionary<EStatsType, int> Stats => stats;


    public void AddStat(EStatsType type, int value)
    {
        if (stats.TryGetValue(type, out _))
            stats[type] += value;
        else
            stats[type] = value;
    }

    public void ResetStats()
        => stats.Clear();
}
