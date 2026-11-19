using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Stat/RuleData")]
public class StatRulesData : ScriptableObject
{
    [SerializeField] private List<StatRule> rules = new();
    [NonSerialized] private StatRule[] ruleLookup;

    private static readonly StatType[] AllStatTypes =
        (StatType[])Enum.GetValues(typeof(StatType));

    private void OnValidate()
    {
        List<StatRule> newRules = new();

        for (int i = 0; i < AllStatTypes.Length; i++)
        {
            bool isAdded = false;

            for (int j = 0; j < rules.Count; j++)
            {
                if (rules[j].StatType == AllStatTypes[i])
                {
                    if (!isAdded)
                    {
                        newRules.Add(rules[j]);
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
                Debug.LogWarning($"[{name}] {AllStatTypes[i]} 규칙이 없어 기본값으로 추가했습니다. 값을 채워 주세요.", this);
                newRules.Add(CreateDefaultStatRule(AllStatTypes[i]));
            }
        }

        rules = newRules;
        
        for (int i = 0; i < rules.Count; i++)
        {
            rules[i].RefreshName();
        }
        
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
        return new StatRule(
            statType,
            StatModifierType.Flat,
            StatRoundingMode.None,
            0f);
    }
}