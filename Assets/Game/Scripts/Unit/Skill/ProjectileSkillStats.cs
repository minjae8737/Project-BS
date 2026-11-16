public struct ProjectileSkillStats
{
    public float DamagePer;
    public float CooldownReductionRate;
    public int Quantity;
    public float Speed;
    public float MaxDistance;
    public float Size;
    public int Penetration;

    public ProjectileSkillStats(ProjectileSkillData data)
    {
        DamagePer = data.DamagePer;
        CooldownReductionRate = data.Cooldown;
        Quantity = data.Quantity;
        Speed = data.Speed;
        MaxDistance = data.MaxDistance;
        Size = data.Size;
        Penetration = data.Penetration;
    }
}