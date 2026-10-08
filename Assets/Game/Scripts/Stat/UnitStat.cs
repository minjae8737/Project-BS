using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitStat : MonoBehaviour
{
    [SerializeField] private StatData statData;
    private Dictionary<StatType, StatSlot> stats;
    private static readonly StatType[] AllStatTypes = 
        (StatType[])Enum.GetValues(typeof(StatType));

    private int hp;

    public int Hp
    {
        get
        {
            return hp;
        }
        private set
        {
            hp = (int)Mathf.Clamp(value, 0f, stats[StatType.MaxHp].FinalValue);
        }
    }
    public float MoveSpeed => stats[StatType.MoveSpeed].FinalValue;
    public float BaseSpeed => stats[StatType.MoveSpeed].BaseValue;
    public bool IsDead => hp <= 0;
    
    public event Action Died;
    public event Action Damaged;

    private void Awake()
    {
        Assert.IsNotNull(statData, $"{gameObject.name} 에 StatData가 할당되어야 합니다.");

        stats = new();
    }
    
    public void Init(StatRulesData statRulesData)
    {
        foreach (StatType statType in AllStatTypes)
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

        hp = (int)stats[StatType.MaxHp].FinalValue;
    }

    public void TakeDamage(float rawDamage)
    {
        if (IsDead) return;

        int damage = DamageCalculator.CalculateDamage(rawDamage, stats[StatType.Defence].FinalValue);
        Hp -= damage;
        
        Damaged?.Invoke();
        
        if (IsDead) 
            Died?.Invoke();
    }
}