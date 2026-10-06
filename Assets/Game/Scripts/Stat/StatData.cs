using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/StatData")]
public class StatData : ScriptableObject
{
    public List<Stat> Stats = new();
}