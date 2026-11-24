using System;

public struct StatChange<TEnum> where TEnum : struct, Enum
{
    public TEnum Type;
    public float InputValue;
    public float CurrentValue;
    public float AfterValue;
}