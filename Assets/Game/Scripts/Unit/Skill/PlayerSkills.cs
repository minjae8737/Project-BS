using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private List<SkillData> datas;
    private List<Skill> skills = new();
    
    

    public void TryExecute(float deltaTime)
    {
        foreach (Skill skill in skills)
        {
            skill.TryExecute(deltaTime);
        }
    }

    public void Tick(Vector3 caster, float deltaTime)
    {
        TryExecute(deltaTime);
    }
}