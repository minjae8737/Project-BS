using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private List<SkillData> datas;
    [SerializeField] private Vector3 skillStartOffset;
    private List<Skill> skills = new();

    public void Init()
    {
        foreach (SkillData data in datas)
        {
            var skill = data.CreateSkill();
            skills.Add(skill);
        }
    }

    public void Tick(Vector3 caster, float attackPower, float deltaTime)
    {
        SkillContext context = new SkillContext
        {
            Caster = caster,
            SkillStartPos = caster + skillStartOffset,
            AttackPower = attackPower
        };

        foreach (Skill skill in skills)
        {
            skill.Tick(context, deltaTime);
        }
    }
}