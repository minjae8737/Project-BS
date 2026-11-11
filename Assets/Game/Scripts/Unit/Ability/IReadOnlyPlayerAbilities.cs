using System.Collections.Generic;

public interface IReadOnlyPlayerAbilities
{
    public IReadOnlyList<AbilitySlot> AbilitySlots { get; }
    public bool HasEmptySlot(AbilityType abilityType);
    public bool TryGetLevel(AbilityData abilityData, out int level);
}