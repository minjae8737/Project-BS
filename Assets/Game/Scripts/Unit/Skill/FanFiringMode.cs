using UnityEngine;

public class FanFiringMode : FiringMode
{
    private FanFiringModeData data;

    public FanFiringMode(FanFiringModeData data, IProjectileLauncher projectileLauncher) : base(projectileLauncher)
    {
        this.data = data;
    }

    public override void Execute(SkillContext context, int projectileCount)
    {
        bool isFullCircle = projectileCount * data.GapAngle >= 360f;
        float startAngle;
        float gapAngle;
        
        if (isFullCircle)
        {
            startAngle = 0f;
            gapAngle = 360f / projectileCount;
        }
        else
        {
            startAngle = -(data.GapAngle / 2) * (projectileCount - 1);
            gapAngle = data.GapAngle;
        }

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + (i * gapAngle);
            Vector3 moveDir = Quaternion.AngleAxis(angle, Vector3.up) * context.Forward;
            projectileLauncher.SpawnProjectile(context, moveDir);
        }
    }
}