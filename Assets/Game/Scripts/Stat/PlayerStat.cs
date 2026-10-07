using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private StatData statData;
    private Dictionary<StatType, StatSlot> stats;

    public int Hp { get; private set; }
    public float MoveSpeed => stats[StatType.MoveSpeed].FinalValue;
    public float BaseSpeed => stats[StatType.MoveSpeed].BaseValue;

    private void Awake()
    {
        Assert.IsNotNull(statData, $"{gameObject.name} 에 StatData가 할당되어야 합니다.");

        stats = new();
    }
    
    public void Init(StatRulesData statRulesData)
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            StatRule rule = statRulesData.GetRule(statType);
            Stat stat = statData.GetStat(statType);
            
            if (!stats.TryGetValue(statType, out StatSlot slot))
            {
                StatSlot newStatSlot = new(stat.Value, rule);
                stats.Add(statType, newStatSlot);
            }
            else
            {
                slot.UpdateValue(stat.Value, rule);
                slot.Reset();
            }
        }

        Hp = (int)stats[StatType.MaxHp].FinalValue;
    }
}