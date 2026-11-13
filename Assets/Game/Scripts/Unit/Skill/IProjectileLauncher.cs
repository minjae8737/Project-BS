using UnityEngine;

public interface IProjectileLauncher
{
    public Collider FindNearestTarget(SkillContext context);
    public void SpawnProjectile(SkillContext context, Vector3 moveDir);
}