public struct PlayerStatDto
{
    public float AttackPower;   // 공격력
    public float AttackSpeed;   // 공격 속도
    public float Area;          // 효과 크기
    public int Amount;          // 투사체 수

    public PlayerStatDto(float attackPower, float attackSpeed, float area, int amount)
    {
        AttackPower = attackPower;
        AttackSpeed = attackSpeed;
        Area = area;
        Amount = amount;
    }
}