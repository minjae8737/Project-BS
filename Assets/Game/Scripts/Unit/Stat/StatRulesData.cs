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
        ruleLookup = new StatRule[AllStatTypes.Length];

        foreach (StatRule rule in rules)
        {
            int index = (int)rule.Type;

            if (ruleLookup[index] != null)
            {
                Debug.LogError($"[{name}] StatRule 중복 : {rule.Type}", this);
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