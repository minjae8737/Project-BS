public struct PlayerStatDto
{
    public float AttackPower;         // 공격력
    public float CooldownReduction;   // 쿨타임 감소
    public float Area;                // 효과 크기
    public int Amount;                // 개수

    public PlayerStatDto(float attackPower, float cooldownReduction, float area, int amount)
    {
        AttackPower = attackPower;
        CooldownReduction = cooldownReduction;
        Area = area;
        Amount = amount;
    }
}