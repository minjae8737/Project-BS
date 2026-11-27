public class RewardOption
{
    private AbilityData data;
    private int nextLevel;
    private string description;

    public AbilityData Data => data;
    public int NextLevel => nextLevel;
    public string Description => description;

    public RewardOption(AbilityData data, int nextLevel)
    {
        this.data = data;
        this.nextLevel = nextLevel;
    }

    public void SetDescription(string text)
    {
        description = text;
    }
}