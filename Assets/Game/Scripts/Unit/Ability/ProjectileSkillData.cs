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

    public float DamagePercent => damageRate;
    public float DamageRate
    {
        get => damageRate / 100f;
    }

    public float CooldownReductionPercent => cooldownReductionRate;
    public float CooldownReductionRate
    {
        get => cooldownReductionRate / 100f;
    }

    public float SpeedPercent => speedRate;
    public float SpeedRate
    {
        get => speedRate / 100f;
    }

    public int Quantity => quantity;

    public int Penetration => penetration;

    public float SizePercent => sizeRate;
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
    [SerializeField] private FiringModeData firingModeData;
    [SerializeField, Min(1)] private int quantity = 1;
    [SerializeField, Min(0)] private float speed;
    [SerializeField, Min(0)] private float maxDistance;
    [SerializeField, Min(0.1f)] private float size = 1f;
    [SerializeField, Min(0)] private int penetration;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private List<ProjectileLevelBonus> levelBonus;

    private const int BonusRowCount = MaxLevel - 1;

    public FiringModeData FiringModeData => firingModeData;
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

    public override bool TryFillStatChanges(int nextLevel, List<StatChange<SkillStatType>> statChanges)
    {
        if (statChanges == null)
        {
            Debug.LogError($"[{name}] statChanges가 null입니다.", this);
            return false;
        }
        statChanges.Clear();
        
        if (!TryCalculateSkillStats(nextLevel, out var nextSkillStats)) 
            return false;

        if (!TryCalculateSkillStats(nextLevel - 1, out var currentSkillStats))
            return false;

        int bonusIndex = nextLevel - 2;

        ProjectileLevelBonus bonus = levelBonus[bonusIndex];
        AddIfIncreased(SkillStatType.DamagePer, bonus.DamagePercent, currentSkillStats.DamagePer, nextSkillStats.DamagePer);
        AddIfIncreased(SkillStatType.CooldownReduction, bonus.CooldownReductionPercent, currentSkillStats.CooldownReductionRate, nextSkillStats.CooldownReductionRate);
        AddIfIncreased(SkillStatType.Speed, bonus.SpeedPercent, currentSkillStats.Speed, nextSkillStats.Speed);
        AddIfIncreased(SkillStatType.Quantity, bonus.Quantity, currentSkillStats.Quantity, nextSkillStats.Quantity);
        AddIfIncreased(SkillStatType.Penetration, bonus.Penetration, currentSkillStats.Penetration, nextSkillStats.Penetration);
        AddIfIncreased(SkillStatType.Size, bonus.SizePercent, currentSkillStats.Size, nextSkillStats.Size);
        
        return true;

        void AddIfIncreased(SkillStatType skillStatType, float inputValue, float currentValue, float afterValue)
        {
            if (inputValue > 0)
            {
                StatChange<SkillStatType> statChange = new StatChange<SkillStatType>(
                    skillStatType,
                    inputValue,
                    currentValue,
                    afterValue
                );
                statChanges.Add(statChange);
            }
        }
    }

    private void OnValidate()
    {
        if (FiringModeData == null)
        {
            Debug.LogError($"[{name}] {DisplayName} 의 FiringModeData가 비어있습니다.", this);
        }
        
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

    public override string GetLevelBonusDescription(int level)
    {
        int bonusIndex = level - 2;

        if (level <= 1) return Description;
        if (bonusIndex < 0 || bonusIndex >= LevelBonus.Count) return string.Empty;

        return LevelBonus[bonusIndex].Description;
    }

    public bool TryCalculateSkillStats(int level, out ProjectileSkillStats skillStats)
    {
        skillStats = new(this);

        if (level < 1 || level > MaxLevel) return false;

        int lastBonusIndex = level - 2;

        float damageRateSum = 0f;
        float cooldownReductionRateSum = 0f;
        int quantitySum = 0;
        float speedRateSum = 0f;
        float sizeRateSum = 0f;
        int penetrationSum = 0;

        for (int i = 0; i <= lastBonusIndex; i++)
        {
            damageRateSum += levelBonus[i].DamageRate;
            cooldownReductionRateSum += levelBonus[i].CooldownReductionRate;
            quantitySum += levelBonus[i].Quantity;
            speedRateSum += levelBonus[i].SpeedRate;
            sizeRateSum += levelBonus[i].SizeRate;
            penetrationSum += levelBonus[i].Penetration;
        }
        
        skillStats.DamagePer *= (1 + damageRateSum);
        skillStats.CooldownReductionRate = cooldownReductionRateSum;
        skillStats.Quantity += quantitySum;
        skillStats.Speed *= (1 + speedRateSum);
        skillStats.Size *= (1 + sizeRateSum);
        skillStats.Penetration += penetrationSum;

        return true;
    }
}