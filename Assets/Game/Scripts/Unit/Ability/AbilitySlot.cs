public class AbilitySlot
{
    private AbilityData data;
    private int level;
    
    public AbilityData Data => data;
    public int Level => level;
    public bool IsMaxLevel => level >= AbilityData.MaxLevel;
}