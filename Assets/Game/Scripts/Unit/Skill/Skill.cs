using System;
using UnityEngine;

[Serializable]
public class Skill
{
    [SerializeField] protected SkillData data;
    protected float elapsed;

    protected virtual bool CanExecute(float deltaTime)
    {
        elapsed += deltaTime;

        return data.Cooldown < elapsed;
    }

    public virtual void TryExecute(float deltaTime) { }
}