using System;
using UnityEngine;

public enum StatRoundingMode
{
    None,
    Round
}

[Serializable]
public class StatRule : IEnumRow<StatType>
{
    [SerializeField] private string statTypeName;
    [SerializeField, HideInInspector] private StatType statType;
    [SerializeField] private StatModifierType statModifierType;
    [SerializeField] private StatRoundingMode statRoundingMode;
    [SerializeField] private float minValue;
    
    public string StatTypeName => statTypeName;
    public StatType Type => statType;
    public StatModifierType StatModifierType => statModifierType;
    public StatRoundingMode StatRoundingMode => statRoundingMode;
    public float MinValue => minValue;

    public StatRule() { }


    public StatRule(StatType type, StatModifierType statModifierType, StatRoundingMode statRoundingMode, float minValue)
    {
        statTypeName = type.ToString();
        statType = type;
        this.statModifierType = statModifierType;
        this.statRoundingMode = statRoundingMode;
        this.minValue = minValue;
        RefreshName();
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

    public void RefreshName()
    {
        statTypeName = statType.ToString();
    }
}