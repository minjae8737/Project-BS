using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveLevelBonus
{
    [SerializeField, Tooltip("이 레벨에서 더해지는 양")] private List<StatModifier> modifiers;
    [SerializeField, TextArea, Tooltip("레벨별 증가량")] private string description;
    
    public IReadOnlyList<StatModifier> Modifiers => modifiers;
    public string Description => description;
}

[CreateAssetMenu(menuName = "Ability/Passive")]
public class PassiveData : AbilityData
{
    [SerializeField] private List<PassiveLevelBonus> levelBonus;

    public IReadOnlyList<PassiveLevelBonus> LevelBonus => levelBonus;
    private static int maxOptionCount = MaxLevel;
    public override AbilityType Type => AbilityType.Passive;

    private void OnValidate()
    {
        if (levelBonus.Count > maxOptionCount)
        {
            for (int i = levelBonus.Count - 1; i >= maxOptionCount; i--)
            {
                levelBonus.RemoveAt(i);
            }
        }
        else if (levelBonus.Count < maxOptionCount)
        {
            int diffCount = maxOptionCount - levelBonus.Count;
            
            for (int i = 0; i < diffCount; i++)
            {
                levelBonus.Add(new PassiveLevelBonus());
            }
        }
    }
}