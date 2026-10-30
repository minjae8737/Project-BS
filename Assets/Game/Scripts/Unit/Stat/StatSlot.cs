using System.Collections.Generic;
using UnityEngine;

public class StatSlot
{
    private float baseValue;
    private float finalValue;
    private StatRule rule;
    private List<StatModifier> modifiers = new();
    private bool isDirty;

    public float BaseValue => baseValue;

    public float FinalValue
    {
        get
        {
            if (isDirty)
                ApplyModifiers();

            return finalValue;
        }
    }

    public StatSlot(float baseValue, StatRule rule)
    {
        this.baseValue = baseValue;
        this.rule = rule;
        isDirty = true;
    }

    public void UpdateValue(float baseValue, StatRule rule)
    {
        this.baseValue = baseValue;
        this.rule = rule;
        isDirty = true;
    }

    public void AddModifier(StatModifier modifier)
    {
        if (modifier == null)
        {
            Debug.LogError($"StatSlot: {rule.StatType} 스탯 적용 실패");
            return;
        }
        
        isDirty = true;
        modifiers.Add(modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        if (modifiers.Remove(modifier))
            isDirty = true;
    }

    public void Reset()
    {
        isDirty = true;
        modifiers.Clear();
    }

    private void ApplyModifiers()
    {
        if (isDirty)
        {
            float flatValue = 0f;
            float percentValue = 0f;

            foreach (StatModifier modifier in modifiers)
            {
                switch (modifier.StatModifierType)
                {
                    case StatModifierType.Flat:
                        flatValue += modifier.Value;
                        break;
                    case StatModifierType.Percent:
                        percentValue += modifier.Value;
                        break;
                }
            }

            float statValue = (baseValue + flatValue) * (1 + percentValue);
            finalValue = rule.Apply(statValue);

            isDirty = false;
        }
    }
}