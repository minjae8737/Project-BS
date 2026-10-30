using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Stat/RuleData")]
public class StatRulesData : ScriptableObject
{
    [SerializeField] private List<StatRule> rules = new();
    private StatRule[] ruleLookup;

    private static readonly StatType[] AllStatTypes = 
        (StatType[])Enum.GetValues(typeof(StatType));
    
    public StatRule GetRule(StatType statType)
    {
        if (ruleLookup.Length < 1)
            BuildLookup();

        return ruleLookup[(int)statType];
    }

    private void BuildLookup()
    {
        ruleLookup = new StatRule[AllStatTypes.Length];

        foreach (StatRule rule in rules)
        {
            int index = (int)rule.StatType;

            if (ruleLookup[index] != null)
            {
                Debug.LogError($"[{name}] StatRule 중복 : {rule.StatType}", this);
                continue;
            }
            
            ruleLookup[index] = rule;
        }

        for (int i = 0; i < ruleLookup.Length; i++)
        {
            if (ruleLookup[i] != null) continue;

            Debug.LogError($"[{name}] StatRule이 존재하지 않음. 기본규칙을 추가. : {AllStatTypes[i]}", this);

            ruleLookup[i] = CreateDefaultStatRule(AllStatTypes[i]);
        }
    }

    private StatRule CreateDefaultStatRule(StatType statType)
    {
        return new StatRule()
        {
            StatType = statType,
            StatRoundingMode = StatRoundingMode.None,
            MinValue = 0f
        };
    }
}