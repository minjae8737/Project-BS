public abstract class Skill
{
    protected SkillData data;
    private float elapsed;
    public abstract float CurrentCooldown { get; }

    public Skill(SkillData data)
    {
        this.data = data;
        elapsed = 0f;
    }

    private bool CanExecute(float deltaTime)
    {
        if (CurrentCooldown < elapsed) return true;
        elapsed += deltaTime;
        return CurrentCooldown < elapsed;
    }

    public void Tick(SkillContext context, float deltaTime)
    {
        if (CanExecute(deltaTime))
        {
            bool isSuccess = TryExecute(context);
            
            if (isSuccess)
                elapsed -= CurrentCooldown;
        }
        
        OnTick(deltaTime);
    }

    public void SetLevel(int level)
    {
        OnLevelChanged(level);
    }
    
    protected abstract void OnLevelChanged(int level);

    protected abstract bool TryExecute(SkillContext context);

    protected virtual void OnTick(float deltaTime) { }
}