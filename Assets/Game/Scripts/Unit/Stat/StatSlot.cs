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

    public float GetModifierPreview(StatModifier modifier)
    {
        if (modifier == null || modifier.StatType != rule.StatType)
        {
            Debug.LogError($"StatSlot: {rule.StatType} 스탯 미리보기 적용 실패");
            return -1f;
        }
        
        return CalculateStatValue(modifier);
    }

    public void AddModifier(StatModifier modifier)
    {
        if (modifier == null || modifier.StatType != rule.StatType)
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
        finalValue = CalculateStatValue();

        isDirty = false;
    }

    private float CalculateStatValue(StatModifier additionalModifier = null)
    {
        float flatValue = 0f;
        float percentAddValue = 0f;
        float percentMulValue = 1f;

        if (rule.StatModifierType == StatModifierType.Flat)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                flatValue += modifiers[i].InputValue;
            }

            if (additionalModifier != null)
            {
                flatValue += additionalModifier.InputValue;
            }
        }
        else if (rule.StatModifierType == StatModifierType.PercentAdd)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                percentAddValue += modifiers[i].InputValue / 100f;
            }

            if (additionalModifier != null)
            {
                percentAddValue += additionalModifier.InputValue / 100f;
            }
        }
        else if (rule.StatModifierType == StatModifierType.PercentMul)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                percentMulValue *= 1 + modifiers[i].InputValue / 100f;
            }

            if (additionalModifier != null)
            {
                percentMulValue *= 1 + additionalModifier.InputValue / 100f;
            }
        }
        
        float statValue = (baseValue + flatValue) * (1 + percentAddValue) * percentMulValue;
        
        // Debug.Log($"{rule.StatType} : {finalValue} -> {rule.Apply(statValue)}");
        return rule.Apply(statValue); 
    }
}