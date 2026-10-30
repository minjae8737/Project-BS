using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(float rawDamage, float defence)
    {
        return Mathf.Max(Mathf.CeilToInt((rawDamage * 100f) / (100f + defence)), 0);
    }
}