using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Stat/RuleData")]
public class StatRulesData : ScriptableObject
{
    [SerializeField] private List<StatRule> rules = new();
    [NonSerialized] private StatRule[] ruleLookup;

    private void OnValidate()
    {
        EnumRowSynchronizer.SyncToEnum<StatType, StatRule>(rules, CreateDefaultStatRule, this);

        ruleLookup = null;
    }

    public StatRule GetRule(StatType statType)
    {
        if (ruleLookup == null)
            BuildLookup();

        return ruleLookup[(int)statType];
    }

    private void BuildLookup()
    {
        ruleLookup = EnumRowSynchronizer.BuildLookup<StatType, StatRule>(rules, CreateDefaultStatRule, this);
    }

    private StatRule CreateDefaultStatRule(StatType statType)
    {
        return new StatRule(
            statType,
            StatModifierType.Flat,
            StatRoundingMode.None,
            0f);
    }
}