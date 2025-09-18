public class DummyLevelRewardSource : ILevelRewardSource
{
    private readonly VictoryScreenConfig config;

    public DummyLevelRewardSource(VictoryScreenConfig config)
    {
        this.config = config;
    }
    
    public int CalculateCoinsEarned() => config.DefaultCoins;
}