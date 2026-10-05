using System;

[Serializable]
public class Stat
{
    public int AttackPower { get; set; }
    public int Defence { get; set; }
    public float CriticalRate { get; set; }
    public int MaxHp { get; set; }
    public int HpRegeneration { get; set; }
    public float MoveSpeed { get; set; }
}