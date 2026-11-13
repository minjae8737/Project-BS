using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill, IProjectileLauncher
{
    private List<Projectile> projectiles = new();
    private Collider[] targets = new Collider[32];

    private ProjectileSkillData projectileData;
    private ProjectileSkillStats currentStats;

    public override float CurrentCooldown => currentStats.Cooldown;

    public ProjectileSkill(ProjectileSkillData data) : base(data)
    {
        projectileData = data;
        SetLevel(1);
    }

    private Projectile GetProjectile()
    {
        Projectile newProjectile = PoolManager.Instance.Get(projectileData.ProjectilePrefab);
        projectiles.Add(newProjectile);

        return newProjectile;
    }

    protected override void OnLevelChanged(int level)
    {
        if (projectileData.TryCalculateSkillStats(level, out var newStats))
        {
            currentStats = newStats;
        }
        else
        {
            Debug.LogError($"[ProjectileSkill] {projectileData.DisplayName} 레벨 변경 실패. 실패 Level : {level}");
        }
    }

    protected override bool TryExecute(SkillContext context)
    {
        var nearestTarget = FindNearestTarget(context);

        if (nearestTarget != null)
        {
            SpawnProjectile(context, context.Forward);
            return true;
        }

        return false;
    }

    public Collider FindNearestTarget(SkillContext context)
    {
        int targetCount = Physics.OverlapSphereNonAlloc(context.SkillStartPos, projectileData.AttackRange, targets, projectileData.TargetLayer);

        if (targetCount > 0)
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

        return null;
    }

    public void SpawnProjectile(SkillContext context, Vector3 moveDir)
    {
        Projectile newProjectile = GetProjectile();
        newProjectile.Init(
            context.AttackPower * currentStats.DamagePer,
            context.SkillStartPos,
            moveDir,
            currentStats.Speed,
            currentStats.MaxDistance,
            currentStats.Penetration,
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
                PoolManager.Instance.Release(projectiles[lastIndex]);
                projectiles.RemoveAt(lastIndex);
            }
        }
    }

    public override void Reset()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            PoolManager.Instance.Release(projectiles[i]);
        }

        projectiles.Clear();
    }
}