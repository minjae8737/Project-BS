using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Stat/RuleData")]
public class StatRulesData : ScriptableObject
{
    public List<StatRule> Rules = new();
}