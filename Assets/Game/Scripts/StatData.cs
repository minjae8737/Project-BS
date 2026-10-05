using UnityEngine;

[CreateAssetMenu(menuName = "Character/StatData")]
public class StatData : ScriptableObject
{
    [SerializeField] private int attackPower;
    [SerializeField] private int defence;
    [SerializeField] private float criticalRate;
    [SerializeField] private int maxHp;
    [SerializeField] private int hpRegeneration;
    [SerializeField] private float moveSpeed;

    public float MoveSpeed => moveSpeed;

    public Stat CreateStat()
    {
        return new Stat()
        {
            AttackPower = attackPower,
            Defence = defence,
            CriticalRate = criticalRate,
            MaxHp = maxHp,
            HpRegeneration = hpRegeneration,
            MoveSpeed = moveSpeed
        };
    }
}