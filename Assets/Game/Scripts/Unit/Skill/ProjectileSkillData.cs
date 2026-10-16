using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Data/Projectile")]
public class ProjectileSkillData : SkillData
{
    [Header("Projectile Info")] 
    [SerializeField, Min(0)] private float speed;
    [SerializeField, Min(0)] private float maxDistance;
    [SerializeField, Min(0)] private int penetration;
    [SerializeField] private Projectile projectilePrefab;

    public float Speed => speed;
    public float MaxDistance => maxDistance;
    public int Penetration => penetration;
    public Projectile ProjectilePrefab => projectilePrefab;

    public override Skill CreateSkill()
    {
        return new ProjectileSkill(this); 
    }
}