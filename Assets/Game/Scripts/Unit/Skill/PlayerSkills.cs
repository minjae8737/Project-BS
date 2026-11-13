using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private Vector3 skillStartOffset;
    
    private List<Skill> skills = new();
    private Dictionary<SkillData, Skill> skillsByData = new();

    public void Init()
    {
        foreach (Skill skill in skills)
        {
            skill.Reset();
        }
        
        skills.Clear();
        skillsByData.Clear();
    }

    public void Tick(Vector3 casterPosition, Vector3 forward, float attackPower, float deltaTime)
    {
        forward.y = 0f;
        
        SkillContext context = new SkillContext
        {
            CasterPos = casterPosition,
            Forward = forward.normalized,
            SkillStartPos = casterPosition + skillStartOffset,
            AttackPower = attackPower
        };
        
        foreach (Skill skill in skills)
        {
            skill.Tick(context, deltaTime);
        }
    }

    public void AddSkill(SkillData skillData)
    {
        Skill newSkill = skillData.CreateSkill();
        
        skills.Add(newSkill);
        skillsByData.Add(skillData, newSkill);
    }

    public void SetSkillLevel(SkillData skillData, int level)
    {
        if (!skillsByData.TryGetValue(skillData, out Skill skill))
        {
            Debug.LogError($"[PlayerSkills] {skillData.DisplayName} 은 등록되지 않은 스킬입니다.", this);
            return;
        }

        skill.SetLevel(level);
    }
}