using System;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [SerializeField] private StatType statType;
    [SerializeField] private StatModifierType statModifierType;
    [SerializeField, Tooltip("Flat: 그대로 / Percent: 10 = 10%")] private float inputValue;
    
    public StatType StatType => statType;
    public StatModifierType StatModifierType => statModifierType;
    public float InputValue => inputValue;

    public float AppliedValue
    {
        get
        {
            if (statModifierType == StatModifierType.Percent)
                return inputValue / 100f;

            return inputValue;
        }
    }
}