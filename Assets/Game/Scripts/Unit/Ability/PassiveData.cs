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
        int bonusIndex = level - 1;
        
        if (level <= 0) return Description;
        if (bonusIndex < 0 || bonusIndex >= LevelBonus.Count) return string.Empty;

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

    public bool TryFillStatChanges(int nextLevel, List<StatChange<StatType>> statChanges, IStatPreviewProvider previewProvider)
    {
        if (statChanges == null )
        {
            Debug.LogError($"[{name}] statChanges가 null입니다.", this);
            return false;
        }
        
        statChanges.Clear();

        if (previewProvider == null)
        {
            Debug.LogError($"[{name}] IStatPreviewProvider가 null입니다.", this);
            return false;
        }

        if (!TryGetLevelBonus(nextLevel, out var statModifiers))
            return false;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier statModifier = statModifiers[i];
            
            if (!previewProvider.TryGetModifierPreview(statModifier, out float currentValue, out float resultValue))
            {
                Debug.LogError($"[{name}] {statModifier.StatType}스텟의 Preview를 얻지 못했습니다.", this);
                continue;
            }
            
            AddIfIncreased(statModifier.StatType, statModifier.InputValue, currentValue, resultValue);
        }
        
        return true;

        void AddIfIncreased(StatType statType, float inputValue, float currentValue, float afterValue)
        {
            if (inputValue > 0)
            {
                StatChange<StatType> statChange = new StatChange<StatType>(
                    statType,
                    inputValue,
                    currentValue,
                    afterValue
                );
                statChanges.Add(statChange);
            }
        }
    }
}