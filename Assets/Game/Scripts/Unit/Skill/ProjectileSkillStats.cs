using System;

[Serializable]
public struct ProjectileSkillStats
{
    public float DamagePer;
    public float Cooldown;
    public float AttackRange;
    public int Quantity;
    public float Speed;
    public float MaxDistance;
    public float Size;
    public int Penetration;

    public ProjectileSkillStats(ProjectileSkillData data)
    {
        DamagePer = data.DamagePer;
        Cooldown = data.Cooldown;
        AttackRange = data.AttackRange;
        Quantity = data.Quantity;
        Speed = data.Speed;
        MaxDistance = data.MaxDistance;
        Size = data.Size;
        Penetration = data.Penetration;
    }
}