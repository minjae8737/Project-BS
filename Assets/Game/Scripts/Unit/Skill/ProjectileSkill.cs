using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    private List<Projectile> projectiles = new();
    private Collider[] targets = new Collider[32];

    private ProjectileSkillData projectileData;

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
    
    protected override bool TryExecute(SkillContext context)
    {
        int targetCount = Physics.OverlapSphereNonAlloc(context.SkillStartPos, projectileData.Range, targets, projectileData.TargetLayer);

        if (targetCount > 0)
        {
            var nearestTarget = FindNearestTarget(context, targetCount);

            SpawnProjectile(context, nearestTarget);
            return true;
        }
        
        return false;
    }

    private Collider FindNearestTarget(SkillContext context, int targetCount)
    {
        Collider nearestTarget = targets[0];
        float nearestSqr = (nearestTarget.transform.position - context.SkillStartPos).sqrMagnitude;

        for (int i = 1; i < targetCount; i++)
        {
            float targetSqr = (targets[i].transform.position - context.SkillStartPos).sqrMagnitude;
            if (nearestSqr > targetSqr)
            {
                nearestTarget = targets[i];
                nearestSqr = targetSqr;
            }
        }

        return nearestTarget;
    }

    private void SpawnProjectile(SkillContext context, Collider nearestTarget)
    {
        Projectile newProjectile = GetProjectile();
        Vector3 moveDir = (nearestTarget.transform.position - context.SkillStartPos).normalized;
        newProjectile.Init(
            context.AttackPower * projectileData.DamagePer,
            context.SkillStartPos,
            moveDir,
            projectileData.Speed,
            projectileData.MaxDistance,
            projectileData.Penetration,
            projectileData.TargetLayer,
            projectileData.ObstacleLayer);
    }

    protected override void OnTick(float deltaTime)
    {
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