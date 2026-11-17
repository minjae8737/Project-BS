using System;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [SerializeField] private StatType statType;
    [SerializeField, Tooltip("단위는 스탯 정의의 보정 방식을 따름: Flat = 그대로 더함 / PercentAdd·PercentMul = 10 → 10%")] private float inputValue;
    
    public StatType StatType => statType;
    public float InputValue => inputValue;

    public StatModifier() { }

    public StatModifier(StatType statType, float inputValue)
    {
        this.statType = statType;
        this.inputValue = inputValue;
    }
}