using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : IReadOnlyPlayerAbilities
{
    private List<AbilitySlot> abilitySlots;

    public IReadOnlyList<AbilitySlot> AbilitySlots => abilitySlots;

    private const int MaxActiveSlotCount = 3;
    private const int MaxPassiveSlotCount = 3;

    public void Init()
    {
        abilitySlots = new();
    }

    private bool TryFindAbilitySlot(AbilityData abilityData, out AbilitySlot abilitySlot, out int index)
    {
        abilitySlot = null;
        index = -1;
        
        for (int i = 0; i < abilitySlots.Count; i++)
        {
            if (abilitySlots[i].Data == abilityData)
            {
                abilitySlot = abilitySlots[i];
                index = i;
                return true;
            }
        }

        return false;
    }

    public bool HasEmptySlot(AbilityType abilityType)
    {
        int count = 0;

        for (int i = 0; i < abilitySlots.Count; i++)
        {
            if (abilityType == abilitySlots[i].Data.Type)
                count++;
        }

        return count < (abilityType == AbilityType.Active ? MaxActiveSlotCount : MaxPassiveSlotCount);
    }

    public bool HasAbility(AbilityData abilityData)
    {
        return TryFindAbilitySlot(abilityData, out _, out _);
    }

    public bool TryGetLevel(AbilityData abilityData, out int level)
    {
        level = 0;

        if (TryFindAbilitySlot(abilityData, out var slot, out _))
        {
            level = slot.Level;
            return true;
        }

        return false;
    }

    public bool TryAcquire(AbilityData abilityData)
    {
        if (HasAbility(abilityData) || !HasEmptySlot(abilityData.Type))
            return false;

        AbilitySlot newSlot = new AbilitySlot(abilityData, 1);
        abilitySlots.Add(newSlot);

        return true;
    }

    public bool TryLevelUp(AbilityData abilityData, out int newLevel)
    {
        newLevel = 0;
        if (!TryFindAbilitySlot(abilityData, out AbilitySlot abilitySlot, out var slotIndex))
            return false;

        if (abilitySlot.IsMaxLevel)
            return false;

        newLevel = abilitySlot.Level + 1;

        AbilitySlot newSlot = new AbilitySlot(abilityData, newLevel);
        abilitySlots[slotIndex] = newSlot;

        return true;
    }
}