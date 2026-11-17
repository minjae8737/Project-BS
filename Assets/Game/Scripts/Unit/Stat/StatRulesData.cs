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
        ruleLookup = null;

        if (rules.Count > AllStatTypes.Length)
        {
            for (int i = rules.Count - 1; i >= AllStatTypes.Length; i--)
            {
                rules.RemoveAt(i);
            }
        }
        else if (rules.Count < AllStatTypes.Length)
        {
            for (int i = 0; i < AllStatTypes.Length; i++)
            {
                if (i < rules.Count)
                {
                    if ((StatType)i != rules[i].StatType)
                    {
                        rules.Insert(i, CreateDefaultStatRule((StatType)i));
                    }
                }
                else
                {
                    rules.Insert(i, CreateDefaultStatRule((StatType)i));
                }
            }
        }
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