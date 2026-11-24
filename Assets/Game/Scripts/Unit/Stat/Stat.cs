using System;
using UnityEngine;

/// <summary>
/// 맨 뒤에만 추가, 중간 값은 지우지 않기(안 쓰면 그대로 두기)
/// </summary>
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
public class Stat : IEnumRow<StatType>
{
    [SerializeField] private string statTypeName;
    [SerializeField, HideInInspector] private StatType type;
    [SerializeField] private float value;

    public StatType Type => type;
    public string StatTypeName => statTypeName;
    public float Value => value;

    public Stat() { }


    public Stat(StatType type, float value)
    {
        this.type = type;
        this.value = value;
        RefreshName();
    }

    public void RefreshName()
    {
        statTypeName = type.ToString();
    }
}