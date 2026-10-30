public abstract class Skill
{
    protected SkillData data;
    private float elapsed;

    public Skill(SkillData data)
    {
        this.data = data;
        elapsed = 0f;
    }

    private bool CanExecute(float deltaTime)
    {
        if (data.Cooldown < elapsed) return true;
        elapsed += deltaTime;
        return data.Cooldown < elapsed;
    }

    public void Tick(SkillContext context, float deltaTime)
    {
        if (CanExecute(deltaTime))
        {
            bool isSuccess = TryExecute(context);
            
            if (isSuccess)
                elapsed -= data.Cooldown;
        }
        
        OnTick(deltaTime);
    }

    protected abstract bool TryExecute(SkillContext context);

    protected virtual void OnTick(float deltaTime) { }
}