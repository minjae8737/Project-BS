using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/ChangeText")]
public class StatChangeTextData : ScriptableObject
{
    [SerializeField] private List<StatChangeTextRow<SkillStatType>> skillStatRows = new();
    [SerializeField] private List<StatChangeTextRow<StatType>> statRows = new();
    
    [NonSerialized] private StatChangeTextRow<SkillStatType>[] skillStatLookup;
    [NonSerialized] private StatChangeTextRow<StatType>[] statLookup;
    
    private void OnValidate()
    {
        EnumRowSynchronizer.SyncToEnum<SkillStatType, StatChangeTextRow<SkillStatType>>(skillStatRows, CreateDefaultSkillStatRow, this);
        EnumRowSynchronizer.SyncToEnum<StatType, StatChangeTextRow<StatType>>(statRows, CreateDefaultStatRow, this);

        skillStatLookup = null;
        statLookup = null;
    }
    
    private void BuildLookup()
    {
        skillStatLookup = EnumRowSynchronizer.BuildLookup<SkillStatType, StatChangeTextRow<SkillStatType>>(skillStatRows, CreateDefaultSkillStatRow, this);
        statLookup = EnumRowSynchronizer.BuildLookup<StatType, StatChangeTextRow<StatType>>(statRows, CreateDefaultStatRow, this);
    }

    public string GetSkillStatFormat(SkillStatType skillStatType)
    {
        if (skillStatLookup == null)
            BuildLookup();

        return skillStatLookup[(int)skillStatType].Format;
    }

    public string GetStatFormat(StatType statType)
    {
        if (statLookup == null)
            BuildLookup();

        return statLookup[(int)statType].Format;
    }

    private StatChangeTextRow<SkillStatType> CreateDefaultSkillStatRow(SkillStatType type)
    {
        return new StatChangeTextRow<SkillStatType>(type);
    }
    
    private StatChangeTextRow<StatType> CreateDefaultStatRow(StatType type)
    {
        return new StatChangeTextRow<StatType>(type);
    }
}