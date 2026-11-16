public abstract class FiringMode
{
    protected IProjectileLauncher projectileLauncher;

    public abstract void Execute(SkillContext context, int projectileCount);
    public virtual void Tick(float deltaTime) { }
    public virtual void Reset() { }

    protected FiringMode(IProjectileLauncher projectileLauncher)
    {
        this.projectileLauncher = projectileLauncher;
    }
}