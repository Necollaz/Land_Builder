public class VictoryFlowCoordinator
{
    private readonly GameResultModel gameResultModel;
    private readonly VictoryScreenPresenter victoryPresenter;
    private readonly ILevelRewardSource rewardSource;
    private readonly VictoryScreenConfig config;

    public VictoryFlowCoordinator(GameResultModel gameResultModel, VictoryScreenPresenter victoryPresenter, ILevelRewardSource rewardSource,
        VictoryScreenConfig config)
    {
        this.gameResultModel = gameResultModel;
        this.victoryPresenter = victoryPresenter;
        this.rewardSource = rewardSource;
        this.config = config;

        this.gameResultModel.ResultChanged += ResultChanged;
    }

    private void ResultChanged(GameResultType result)
    {
        if (result != GameResultType.Victory)
            return;

        int coins = rewardSource != null ? rewardSource.CalculateCoinsEarned() : config.DefaultCoins;
        victoryPresenter.OpenWithCoins(coins);
    }
}