using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/StatData")]
public class StatData : ScriptableObject
{
    [SerializeField] private List<Stat> stats = new();
    private Stat[] statLookup; 

    private static readonly StatType[] AllStatTypes = 
        (StatType[])Enum.GetValues(typeof(StatType));
    
    public Stat GetStat(StatType statType)
    {
        if (statLookup.Length < 1)
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
        return new Stat()
        {
            StatType = statType,
            Value = 0f
        };
    }
}