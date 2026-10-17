using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private List<SkillData> datas;
    private List<Skill> skills = new();

    public void Init()
    {
        foreach (SkillData data in datas)
        {
            var skill = data.CreateSkill();
            skills.Add(skill);
        }
    }

    public void Tick(Vector3 caster, float deltaTime)
    {
        caster.y += 1f;
        SkillContext context = new SkillContext { Caster = caster };

        foreach (Skill skill in skills)
        {
            skill.Tick(context, deltaTime);
        }
    }
}