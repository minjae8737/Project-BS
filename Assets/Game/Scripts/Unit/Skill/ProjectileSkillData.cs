using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Data/Projectile")]
public class ProjectileSkillData : SkillData
{
    [Header("Projectile Info")] 
    [Min(0f)] public float Speed;
    [Min(0f)] public float MaxDistance;
    [Min(0f)] public int Penetration;
    public Projectile projectilePrefab;
}