using System.Collections.Generic;

public class PlayerAbilities
{
    private AbilityCatalogData catalog;
    private List<AbilitySlot> abilitySlots;

    private const int MaxActiveSlotCount = 3;
    private const int MaxPassiveSlotCount = 3;

    public void Init(AbilityCatalogData abilityCatalogData)
    {
        catalog = abilityCatalogData;
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

        return count < (abilityType == AbilityType.Skill ? MaxActiveSlotCount : MaxPassiveSlotCount);
    }

    public bool HasAbility(AbilityData abilityData)
    {
        return TryFindAbilitySlot(abilityData, out var abilitySlot);
    }

    public void LevelUp()
    {
        
    }
    
    
}