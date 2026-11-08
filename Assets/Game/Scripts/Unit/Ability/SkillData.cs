using UnityEngine;

public abstract class SkillData : AbilityData
{
    [SerializeField, Min(0)] private float damagePer;
    [SerializeField, Min(0)] private float cooldown;
    [SerializeField, Min(0)] private float attackRange;
    [SerializeField] private LayerMask targetLayer;

    public override AbilityType Type => AbilityType.Skill;
    public float DamagePer => damagePer;
    public float Cooldown => cooldown;
    public float AttackRange => attackRange;
    public LayerMask TargetLayer => targetLayer;

    public abstract Skill CreateSkill();
}