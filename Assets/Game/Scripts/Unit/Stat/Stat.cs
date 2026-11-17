using System;
using UnityEngine;

public enum StatType
{
    AttackPower = 0,        // 공격력
    Defence = 1,            // 방어력
    CriticalRate = 2,       // 크리티컬 확률
    MaxHp = 3,              // 최대 체력
    HpRegeneration = 4,     // 초당 회복 HP
    MoveSpeed = 5,          // 이동 속도
    PickupRadius = 6,       // 획득 범위
    CooldownReduction = 7,  // 쿨타임 감소(%p, 8 = 8%)
    Area = 8,               // 효과 크기(배율, 1 = 100%)
    Amount = 9,             // 개수
}

public enum StatModifierType
{
    Flat = 0,
    PercentAdd = 1,
    PercentMul = 2,
}

[Serializable]
public class Stat
{
    [SerializeField] private string statTypeName;
    [SerializeField] private StatType statType;
    [SerializeField] private float value;
 
    public string StatTypeName => statTypeName;
    public StatType StatType => statType;
    public float Value => value;

    public Stat() { }

    public Stat(StatType statType, float value)
    {
        statTypeName = statType.ToString();
        this.statType = statType;
        this.value = value;
    }
}