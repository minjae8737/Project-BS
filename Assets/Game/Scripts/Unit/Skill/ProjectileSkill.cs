using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    private List<Projectile> projectiles = new();

    protected override bool CanExecute(float deltaTime)
    {
        elapsed += deltaTime;
        ProjectileSkillData skillData = data as ProjectileSkillData;
        
        // Physics.OverlapSphereNonAlloc()

        return data.Cooldown < elapsed;
    }

    public override void TryExecute(float deltaTime)
    {
        if (CanExecute(deltaTime))
        {
            ProjectileSkillData skillData = data as ProjectileSkillData;

            Projectile newProjectile = GameObject.Instantiate(skillData.projectilePrefab);
            projectiles.Add(newProjectile);
        }
    }
}