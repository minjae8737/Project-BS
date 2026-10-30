using System;
using UnityEngine;

public enum StatRoundingMode
{
    None,
    Round
}

[Serializable]
public class StatRule
{
    public StatType StatType;
    public StatRoundingMode StatRoundingMode;
    public float MinValue;

    public float Apply(float value)
    {
        value = Mathf.Max(value, MinValue);
        
        switch (StatRoundingMode)
        {
            case StatRoundingMode.Round:
                value = (float)Math.Round(value, MidpointRounding.AwayFromZero);
                break;
        }

        return value;
    }
}