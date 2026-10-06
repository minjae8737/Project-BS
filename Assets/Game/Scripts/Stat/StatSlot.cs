using System.Collections.Generic;
using UnityEngine;

public class StatSlot
{
    private float baseStat;
    private float minStat;
    private List<StatModifier> modifiers = new();
    private bool isDirty;

    public float StatValue
    {
        get
        {
            if (isDirty)
                ApplyModifiers();

            return StatValue;
        }
        private set
        {
            StatValue = value;
        }
    }

    public StatSlot(float baseStat, float minStat)
    {
        this.baseStat = baseStat;
        this.minStat = minStat;
        isDirty = true;
    }

    public void AddModifier(StatModifier modifier)
    {
        
    }

    public void RemoveModifier(int id)
    {
        
    }

    private void ApplyModifiers()
    {
        if (isDirty)
        {
            float flatValue = 0f;
            float percentValue = 0f;

            foreach (StatModifier modifier in modifiers)
            {
                switch (modifier.StatValueType)
                {
                    case StatValueType.Flat:
                        flatValue += modifier.Value;
                        break;
                    case StatValueType.Percent:
                        percentValue += modifier.Value;
                        break;
                }
            }

            float statValue = (baseStat + flatValue) * (1 + percentValue);
            StatValue = Mathf.Max(statValue, minStat);
            
            isDirty = false;
        }
    }
}