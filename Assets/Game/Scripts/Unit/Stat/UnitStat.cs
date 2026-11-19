using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitStat : MonoBehaviour
{
    [SerializeField] private StatData statData;
    private Dictionary<StatType, StatSlot> statSlots;
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
            hp = (int)Mathf.Clamp(value, 0f, statSlots[StatType.MaxHp].FinalValue);
        }
    }
    
    public float AttackPower => statSlots[StatType.AttackPower].FinalValue;
    public float MoveSpeed => statSlots[StatType.MoveSpeed].FinalValue;
    public float BaseMoveSpeed => statSlots[StatType.MoveSpeed].BaseValue;
    public float PickupRadius => statSlots[StatType.PickupRadius].FinalValue;
    public float CooldownReduction => statSlots[StatType.CooldownReduction].FinalValue / 100f;
    public float Area => statSlots[StatType.Area].FinalValue;
    public int Amount => (int)statSlots[StatType.Amount].FinalValue;
    public bool IsDead => hp <= 0;
    
    public event Action Died;
    public event Action<int> Damaged;

    private void Awake()
    {
        Assert.IsNotNull(statData, $"{gameObject.name} 에 StatData가 할당되어야 합니다.");

        statSlots = new();
    }
    
    public void Init(StatRulesData statRulesData)
    {
        foreach (StatType statType in AllStatTypes)
        {
            StatRule rule = statRulesData.GetRule(statType);
            Stat stat = statData.GetStat(statType);
            
            if (!statSlots.TryGetValue(statType, out StatSlot slot))
            {
                StatSlot newStatSlot = new(stat.Value, rule);
                statSlots.Add(statType, newStatSlot);
            }
            else
            {
                slot.UpdateValue(stat.Value, rule);
                slot.Reset();
            }
        }

        hp = (int)statSlots[StatType.MaxHp].FinalValue;
    }

    public void TakeDamage(float rawDamage)
    {
        if (IsDead) return;

        int damage = DamageCalculator.CalculateDamage(rawDamage, statSlots[StatType.Defence].FinalValue);
        Hp -= damage;
        
        Damaged?.Invoke(damage);
        
        if (IsDead) 
            Died?.Invoke();
    }

    public void AddModifiers(IReadOnlyList<StatModifier> modifiers)
    {
        float preMaxHp = statSlots[StatType.MaxHp].FinalValue;
        
        for (int i = 0; i < modifiers.Count; i++)
        {
            statSlots[modifiers[i].StatType].AddModifier(modifiers[i]);
        }
        
        ApplyMaxHpChange(preMaxHp);
    }
    
    public void RemoveModifiers(IReadOnlyList<StatModifier> modifiers)
    {
        float preMaxHp = statSlots[StatType.MaxHp].FinalValue;

        for (int i = 0; i < modifiers.Count; i++)
        {
            statSlots[modifiers[i].StatType].RemoveModifier(modifiers[i]);
        }
        
        ApplyMaxHpChange(preMaxHp);
    }

    private void ApplyMaxHpChange(float preMaxHp)
    {
        if (IsDead) return;
        
        float postMaxHp = statSlots[StatType.MaxHp].FinalValue;
        int maxHpDelta = Mathf.RoundToInt(postMaxHp - preMaxHp);
        
        Hp += Mathf.Max(maxHpDelta, 0);
    }
    
    public bool TryGetModifierPreview(StatModifier modifier, out float currentValue, out float resultValue)
    {
        if (modifier == null || !statSlots.TryGetValue(modifier.StatType, out var statSlot))
        {
            currentValue = 0f;
            resultValue = 0f;
            return false;
        }
        
        currentValue = statSlot.FinalValue;
        
        return statSlot.TryGetModifierPreview(modifier, out resultValue);
    }
}