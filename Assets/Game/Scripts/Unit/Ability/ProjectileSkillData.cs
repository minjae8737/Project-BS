using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProjectileLevelBonus
{
    [SerializeField, Min(0)] private float damageRate;
    [SerializeField, Min(0)] private float cooldownReductionRate;
    [SerializeField, Min(0)] private float speedRate;
    [SerializeField, Min(0)] private int quantity;
    [SerializeField, Min(0)] private int penetration;
    [SerializeField, Min(0)] private float sizeRate;
    [SerializeField, TextArea] private string description;

    public float DamageRate
    {
        get => damageRate / 100f;
    }

    public float CooldownReductionRate
    {
        get => cooldownReductionRate / 100f;
    }

    public float SpeedRate
    {
        get => speedRate / 100f;
    }

    public int Quantity => quantity;

    public int Penetration => penetration;

    public float SizeRate
    {
        get => sizeRate / 100f;
    }

    public string Description => description;
}

[CreateAssetMenu(menuName = "Ability/Projectile")]
public class ProjectileSkillData : SkillData
{
    [Header("Projectile Info")] 
    [SerializeField, Min(1)] private int quantity = 1;
    [SerializeField, Min(0)] private float speed;
    [SerializeField, Min(0)] private float maxDistance;
    [SerializeField, Min(0.1f)] private float size = 1f;
    [SerializeField, Min(0)] private int penetration;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private List<ProjectileLevelBonus> levelBonus;

    private const int BonusRowCount = MaxLevel - 1;

    public int Quantity => quantity;
    public float Speed => speed;
    public float MaxDistance => maxDistance;
    public float Size => size;
    public int Penetration => penetration;
    public Projectile ProjectilePrefab => projectilePrefab;
    public LayerMask ObstacleLayer => obstacleLayer;
    public IReadOnlyList<ProjectileLevelBonus> LevelBonus => levelBonus;


    public override Skill CreateSkill()
    {
        return new ProjectileSkill(this);
    }

    private void OnValidate()
    {
        if (levelBonus.Count > BonusRowCount)
        {
            for (int i = levelBonus.Count - 1; i >= BonusRowCount; i--)
            {
                levelBonus.RemoveAt(i);
            }
        }
        else if (levelBonus.Count < BonusRowCount)
        {
            int diffCount = BonusRowCount - levelBonus.Count;

            for (int i = 0; i < diffCount; i++)
            {
                levelBonus.Add(new ProjectileLevelBonus());
            }
        }
    }
    
    public bool TryCalculateSkillStats(int level, out ProjectileSkillStats skillStats)
    {
        skillStats = new(this);

        if (level < 1 || level > MaxLevel) return false;

        int lastBonusIndex = level - 2;

        float damageRateSum = 0f;
        float cooldownReductionRate = 0f;
        int QuantitySum = 0;
        float SpeedRateSum = 0f;
        float SizeRateSum = 0f;
        int PenetrationSum = 0;

        for (int i = 0; i <= lastBonusIndex; i++)
        {
            damageRateSum += levelBonus[i].DamageRate;
            cooldownReductionRate += levelBonus[i].CooldownReductionRate;
            QuantitySum += levelBonus[i].Quantity;
            SpeedRateSum += levelBonus[i].SpeedRate;
            SizeRateSum += levelBonus[i].SizeRate;
            PenetrationSum += levelBonus[i].Penetration;
        }

        cooldownReductionRate = Math.Min(cooldownReductionRate, 0.9f);
        
        skillStats.DamagePer *= (1 + damageRateSum);
        skillStats.Cooldown *= (1 - cooldownReductionRate);
        skillStats.Quantity += QuantitySum;
        skillStats.Speed *= (1 + SpeedRateSum);
        skillStats.Size *= (1 + SizeRateSum);
        skillStats.Penetration += PenetrationSum;

        return true;
    }
}