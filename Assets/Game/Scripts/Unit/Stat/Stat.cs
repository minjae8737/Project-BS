using System;

public enum StatType
{
    AttackPower,        // 공격력
    Defence,            // 방어력
    CriticalRate,       // 크리티컬 확률
    MaxHp,              // 최대 체력
    HpRegeneration,     // 체력 재생률
    MoveSpeed,          // 이동 속도
    PickupRadius,       // 획득 범위
    AttackSpeed,        // 공격 속도
    Area,               // 효과 크기
    Amount,             // 투사체 수
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