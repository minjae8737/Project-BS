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

        if (level < 0 || level > MaxLevel) return false;

        int bonusIndex = level - 2;

        for (int i = 0; i < bonusIndex; i++)
        {
            skillStats.DamagePer += levelBonus[i].DamageRate;
            skillStats.Cooldown += levelBonus[i].CooldownReductionRate;
            skillStats.Quantity += levelBonus[i].Quantity;
            skillStats.Speed += levelBonus[i].SpeedRate;
            skillStats.Size += levelBonus[i].SizeRate;
            skillStats.Penetration += levelBonus[i].Penetration;
        }

        return true;
    }
}