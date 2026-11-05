using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveLevelBonus
{
    [SerializeField] private List<Stat> stats;

    [SerializeField, TextArea, Tooltip("레벨별 증가량")] private string description;
}

[CreateAssetMenu(menuName = "Ability/Passive")]
public class PassiveData : AbilityData
{
    [SerializeField] private List<PassiveLevelBonus> levelBonus;

    public IReadOnlyList<PassiveLevelBonus> LevelBonus => levelBonus;

    private static int maxOptionCount = maxLevel;

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