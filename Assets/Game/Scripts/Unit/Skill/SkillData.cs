using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    [SerializeField, Min(0)] private float damagePer;
    [SerializeField, Min(0)] private float cooldown;
    [SerializeField, Min(0)] private float range;
    [SerializeField] private LayerMask targetLayer;

    public float DamagePer => damagePer;
    public float Cooldown => cooldown;
    public float Range => range;
    public LayerMask TargetLayer => targetLayer;

    public abstract Skill CreateSkill();
}