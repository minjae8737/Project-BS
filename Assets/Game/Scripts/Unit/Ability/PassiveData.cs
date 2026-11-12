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
    private const int BonusRowCount = MaxLevel;
    public override AbilityType Type => AbilityType.Passive;

    private void OnValidate()
    {
        if (levelBonus.Count > BonusRowCount)
        {
            for (int i = levelBonus.Count - 1; i >= BonusRowCount; i--)
            {
                levelBonus.RemoveAt(i);
            }
        }
        else if (levelBonus.Count < BonusRowCount)
        {
            int diffCount = BonusRowCount - levelBonus.Count;
            
            for (int i = 0; i < diffCount; i++)
            {
                levelBonus.Add(new PassiveLevelBonus());
            }
        }
    }

    public override string GetLevelBonusDescription(int level)
    {
        if (level > MaxLevel || level >= LevelBonus.Count) return string.Empty;
        if (level <= 0) return Description;

        int bonusIndex = level - 1;
        return LevelBonus[bonusIndex].Description;
    }

    public bool TryGetLevelBonus(int level, out IReadOnlyList<StatModifier> statModifiers)
    {
        statModifiers = null;

        if (level < 1 || level > MaxLevel) return false;

        int bonusIndex = level - 1;
        if (bonusIndex >= LevelBonus.Count) return false;
        
        statModifiers = LevelBonus[bonusIndex].Modifiers;

        return true;
    }
}