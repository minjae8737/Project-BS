using System;
using UnityEngine;

public enum StatRoundingMode
{
    None,
    Round
}

[Serializable]
public class StatRule
{
    [SerializeField] private string statTypeName;
    [SerializeField] private StatType statType;
    [SerializeField] private StatModifierType statModifierType;
    [SerializeField] private StatRoundingMode statRoundingMode;
    [SerializeField] private float minValue;
    
    public string StatTypeName => statTypeName;
    public StatType StatType => statType;
    public StatModifierType StatModifierType => statModifierType;
    public StatRoundingMode StatRoundingMode => statRoundingMode;
    public float MinValue => minValue;

    public StatRule() { }

    public StatRule(StatType statType, StatModifierType statModifierType, StatRoundingMode statRoundingMode, float minValue)
    {
        statTypeName = statType.ToString();
        this.statType = statType;
        this.statModifierType = statModifierType;
        this.statRoundingMode = statRoundingMode;
        this.minValue = minValue;
    }

    public float Apply(float value)
    {
        value = Mathf.Max(value, minValue);
        
        switch (statRoundingMode)
        {
            case StatRoundingMode.Round:
                value = (float)Math.Round(value, MidpointRounding.AwayFromZero);
                break;
        }

        return value;
    }
}