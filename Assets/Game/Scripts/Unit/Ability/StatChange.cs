using System;

public struct StatChange<TEnum> where TEnum : struct, Enum
{
    public TEnum Type;
    public float InputValue;
    public float CurrentValue;
    public float AfterValue;
    
    public StatChange(TEnum type, float inputValue, float currentValue, float afterValue)
    {
        Type = type;
        InputValue = inputValue;
        CurrentValue = currentValue;
        AfterValue = afterValue;
    }
}