using UnityEngine;

public abstract class SkillData : AbilityData
{
    [SerializeField, Min(0)] private float damagePer;
    [SerializeField, Min(0)] private float cooldown;
    [SerializeField, Min(0)] private float attackRange;
    [SerializeField] private LayerMask targetLayer;

    protected const float MaxCooldownReductionRate = 0.9f;
    public override AbilityType Type => AbilityType.Active;
    public float DamagePer => damagePer;
    public float Cooldown => cooldown;
    public float AttackRange => attackRange;
    public LayerMask TargetLayer => targetLayer;
    
    public abstract Skill CreateSkill();
}