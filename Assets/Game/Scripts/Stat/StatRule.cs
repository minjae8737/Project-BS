using System;

public enum ValueType
{
    Int,
    Float
}

[Serializable]
public class StatRule
{
    public StatType StatType;
    public ValueType ValueType;
    public float MinValue;
}