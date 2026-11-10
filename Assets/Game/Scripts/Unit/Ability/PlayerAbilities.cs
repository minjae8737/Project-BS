using System.Collections.Generic;

public class PlayerAbilities
{
    private List<AbilitySlot> abilitySlots;

    public IReadOnlyList<AbilitySlot> AbilitySlots => abilitySlots;

    private const int MaxActiveSlotCount = 3;
    private const int MaxPassiveSlotCount = 3;

    public void Init()
    {
        abilitySlots = new();
    }

    private bool TryFindAbilitySlot(AbilityData abilityData, out AbilitySlot abilitySlot)
    {
        abilitySlot = null;

        for (int i = 0; i < abilitySlots.Count; i++)
        {
            if (abilitySlots[i].Data == abilityData)
            {
                abilitySlot = abilitySlots[i];
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
        return TryFindAbilitySlot(abilityData, out var _);
    }

    public bool TryAcquire(AbilityData abilityData)
    {
        if (HasAbility(abilityData) || !HasEmptySlot(abilityData.Type))
            return false;

        AbilitySlot newSlot = new AbilitySlot(abilityData, 1);
        abilitySlots.Add(newSlot);

        return true;
    }

    public bool TryLevelUp(AbilityData abilityData, out int newlevel)
    {
        newlevel = 0;

        if (!TryFindAbilitySlot(abilityData, out AbilitySlot abilitySlot))
            return false;

        if (abilitySlot.IsMaxLevel) 
            return false;

        newlevel = abilitySlot.Level + 1;
        
        AbilitySlot newSlot = new AbilitySlot(abilityData, newlevel);
        abilitySlots.Remove(abilitySlot);
        abilitySlots.Add(newSlot);
        
        return true;
    }
}