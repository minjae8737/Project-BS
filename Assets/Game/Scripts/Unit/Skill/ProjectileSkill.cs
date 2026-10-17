using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    private List<Projectile> projectiles = new();
    private Collider[] targets = new Collider[32];

    private ProjectileSkillData projectileData;
    private int targetCount;
    
    public ProjectileSkill(ProjectileSkillData data) : base(data)
    {
        projectileData = data;
    }

    private Projectile GetProjectile()
    {
        Projectile newProjectile = GameObject.Instantiate(projectileData.ProjectilePrefab);
        projectiles.Add(newProjectile);

        return newProjectile;
    }

    public override void Tick(SkillContext context, float deltaTime)
    {
        if (CanExecute(context, deltaTime))
        {
            targetCount = Physics.OverlapSphereNonAlloc(context.Caster, projectileData.Range, targets, projectileData.TargetLayer);
            
            if (targetCount <= 0) return;
            elapsed -= data.Cooldown;
            
            Collider nearestTarget = targets[0];
            float nearestSqr = (nearestTarget.transform.position - context.Caster).sqrMagnitude;

            for (int i = 1; i < targetCount; i++)
            {
                float targetSqr = (targets[i].transform.position - context.Caster).sqrMagnitude;
                if (nearestSqr > targetSqr)
                {
                    nearestTarget = targets[i];
                    nearestSqr = targetSqr;
                }
            }

            Projectile newProjectile = GetProjectile();
            Vector3 moveDir = (nearestTarget.transform.position - context.Caster).normalized;
            newProjectile.Init(
                context.Caster,
                moveDir,
                projectileData.Speed,
                projectileData.MaxDistance,
                projectileData.Penetration,
                projectileData.TargetLayer);
            
        }
        
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Tick(deltaTime);

            if (projectiles[i].IsReached)
            {
                int lastIndex = projectiles.Count - 1;
                (projectiles[i], projectiles[lastIndex]) = (projectiles[lastIndex], projectiles[i]);
                projectiles[lastIndex].gameObject.SetActive(false);
                projectiles.RemoveAt(lastIndex);
            }
        }
    }
}