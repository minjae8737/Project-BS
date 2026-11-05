using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProjectileLevelBonus
{
    [SerializeField, Min(0)] private float damagePercent;
    [SerializeField, Min(0)] private float cooldownPercent;
    [SerializeField, Min(0)] private float speedPercent;
    [SerializeField, Min(0)] private int quantity;
    [SerializeField, Min(0)] private int penetration;
    [SerializeField, Min(0)] private float sizePercent;
    [SerializeField, TextArea] private string description;
}

[CreateAssetMenu(menuName = "Ability/Projectile")]
public class ProjectileSkillData : SkillData
{
    [Header("Projectile Info")] 
    [SerializeField, Min(1)] private int quantity;
    [SerializeField, Min(0)] private float speed;
    [SerializeField, Min(0)] private float maxDistance;
    [SerializeField, Min(1f)] private float size;
    [SerializeField, Min(0)] private int penetration;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private List<ProjectileLevelBonus> levelBonus;
    
    private static int maxOptionCount = maxLevel - 1;

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
        if (levelBonus.Count > maxOptionCount)
        {
            for (int i = levelBonus.Count - 1; i >= maxOptionCount; i--)
            {
                levelBonus.RemoveAt(i);
            }
        }
        else if (levelBonus.Count < maxOptionCount)
        {
            int diffCount = maxOptionCount - levelBonus.Count;
            
            for (int i = 0; i < diffCount; i++)
            {
                levelBonus.Add(new ProjectileLevelBonus());
            }
        }
    }
}