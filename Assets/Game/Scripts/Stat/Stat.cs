using System;

public enum StatType
{
    AttackPower,
    Defence,
    CriticalRate,
    MaxHp,
    HpRegeneration,
    MoveSpeed
}

public enum StatValueType
{
    Flat,
    Percent
}

[Serializable]
public class Stat
{
    public StatType StatType;
    public float Value;
}