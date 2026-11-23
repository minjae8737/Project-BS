using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/StatData")]
public class StatData : ScriptableObject
{
    [SerializeField] private List<Stat> stats = new();
    [NonSerialized] private Stat[] statLookup;

    private void OnValidate()
    {
        EnumRowSynchronizer.SyncToEnum<StatType, Stat>(stats, CreateDefaultStat, this);

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
        statLookup = EnumRowSynchronizer.BuildLookup<StatType, Stat>(stats, CreateDefaultStat, this);
    }

    private Stat CreateDefaultStat(StatType statType)
    {
        return new Stat(statType, 0f);
    }
}