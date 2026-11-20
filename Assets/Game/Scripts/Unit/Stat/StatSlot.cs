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

    public bool TryGetModifierPreview(StatModifier modifier, out float resultValue)
    {
        if (modifier == null || modifier.StatType != rule.StatType)
        {
            Debug.LogError($"StatSlot: {rule.StatType} 스탯 미리보기 적용 실패");
            resultValue = -1f;
            return false;
        }

        resultValue = CalculateStatValue(modifier);
        return true;
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

        foreach (StatModifier modifier in modifiers)
        {
            Accumulate(modifier);
        }

        if (additionalModifier != null)
        {
            Accumulate(additionalModifier);
        }
        
        float statValue = (baseValue + flatValue) * (1 + percentAddValue) * percentMulValue;

        return rule.Apply(statValue);

        void Accumulate(StatModifier modifier)
        {
            switch (rule.StatModifierType)
            {
                case StatModifierType.Flat:
                    flatValue += modifier.InputValue;
                    break;
                case StatModifierType.PercentAdd:
                    percentAddValue += modifier.InputValue / 100f;
                    break;
                case StatModifierType.PercentMul:
                    percentMulValue *= 1 + modifier.InputValue / 100f;
                    break;
                default:
                    Debug.LogError($"[{this} {rule.StatType}] StatModifierType이 계산되지 않는 타입입니다.");
                    break;
            }
        }
    }
}