public abstract class Skill
{
    protected SkillData data;
    protected float elapsed;

    public Skill(SkillData data)
    {
        this.data = data;
        elapsed = 0f;
    }

    protected virtual bool CanExecute(SkillContext context, float deltaTime)
    {
        if (data.Cooldown < elapsed) return true;
        elapsed += deltaTime;
        return data.Cooldown < elapsed;
    }

    public abstract void Tick(SkillContext context, float deltaTime);
}