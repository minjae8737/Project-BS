using UnityEngine;

public abstract class Skill
{
    protected SkillData data;
    private float elapsed;
    protected abstract float CurrentCooldownReductionRate { get; }
    protected const float MaxCooldownReductionRate = 0.9f;
    
    public Skill(SkillData data)
    {
        this.data = data;
        elapsed = 0f;
    }

    private bool CanExecute(float cooldown, float deltaTime)
    {
        if (cooldown < elapsed) return true;
        elapsed += deltaTime;
        return cooldown < elapsed;
    }

    public void Tick(SkillContext context, float deltaTime)
    {
        float cooldownReductionRate = Mathf.Min(CurrentCooldownReductionRate + context.PlayerStatDto.AttackSpeed, MaxCooldownReductionRate);
        float cooldown = data.Cooldown * (1 - cooldownReductionRate);

        if (CanExecute(cooldown, deltaTime))
        {
            bool isSuccess = TryExecute(context);

            if (isSuccess)
                elapsed -= cooldown;
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
    
    public abstract void Reset();
}