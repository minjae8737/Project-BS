using System;

public enum StatType
{
    AttackPower,
    Defence,
    CriticalRate,
    MaxHp,
    HpRegeneration,
    MoveSpeed,
    PickupRadius,
    AttackSpeed,
    SkillRange
}

public enum StatModifierType
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