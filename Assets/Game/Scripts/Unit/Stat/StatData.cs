using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/StatData")]
public class StatData : ScriptableObject
{
    [SerializeField] private List<Stat> stats = new();
    [NonSerialized] private Stat[] statLookup;

    private static readonly StatType[] AllStatTypes =
        (StatType[])Enum.GetValues(typeof(StatType));

    private void OnValidate()
    {
        List<Stat> newStats = new();

        for (int i = 0; i < AllStatTypes.Length; i++)
        {
            bool isAdded = false;

            for (int j = 0; j < stats.Count; j++)
            {
                if (stats[j].StatType == AllStatTypes[i])
                {
                    if (!isAdded)
                    {
                        newStats.Add(stats[j]);
                        isAdded = true;
                    }
                    else
                    {
                        Debug.LogError($"[{name}] {AllStatTypes[i]}은 중복되었습니다.", this);
                    }
                }
            }

            if (!isAdded)
            {
                Debug.LogWarning($"[{name}] {AllStatTypes[i]} 스텟이 없어 기본값으로 추가했습니다. 값을 채워 주세요.", this);
                newStats.Add(CreateDefaultStat(AllStatTypes[i]));
            }
        }

        stats = newStats;
        
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].RefreshName();
        }
        
        statLookup = null;
    }
    
    public Stat GetStat(StatType statType)
    {
        if (statLookup == null)
            BuildLookup();

        return statLookup[(int)statType];
    }

    private void BuildLookup()
    {
        statLookup = new Stat[AllStatTypes.Length];

        foreach (Stat stat in stats)
        {
            int index = (int)stat.StatType;

            if (statLookup[index] != null)
            {
                Debug.LogError($"[{name}] Stat 중복 : {stat.StatType}", this);
                continue;
            }

            statLookup[index] = stat;
        }

        for (int i = 0; i < statLookup.Length; i++)
        {
            if (statLookup[i] != null) continue;

            Debug.LogError($"[{name}] Stat이 존재하지 않음. 기본스텟 추가. : {AllStatTypes[i]}", this);

            statLookup[i] = CreateDefaultStat(AllStatTypes[i]);
        }
    }

    private Stat CreateDefaultStat(StatType statType)
    {
        return new Stat(statType, 0f);
    }
}