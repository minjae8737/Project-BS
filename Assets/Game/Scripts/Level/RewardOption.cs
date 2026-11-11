public class RewardOption
{
    private AbilityData data;
    private int nextLevel;
    
    public AbilityData Data => data;
    public int NextLevel => nextLevel;

    public RewardOption(AbilityData data, int nextLevel)
    {
        this.data = data;
        this.nextLevel = nextLevel;
    }
}