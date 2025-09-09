public class DefeatFlowCoordinator
{
    private readonly GameResultModel gameResultModel;
    private readonly DefeatAttemptModel defeatAttemptModel;
    private readonly DefeatScreenPresenter defeatPresenter;

    public DefeatFlowCoordinator(GameResultModel gameResultModel, DefeatAttemptModel defeatAttemptModel, DefeatScreenPresenter defeatPresenter)
    {
        this.gameResultModel = gameResultModel;
        this.defeatAttemptModel = defeatAttemptModel;
        this.defeatPresenter = defeatPresenter;

        this.gameResultModel.ResultChanged += OnResultChanged;
    }

    private void OnResultChanged(GameResultType result)
    {
        if (result != GameResultType.Defeat)
            return;

        defeatAttemptModel.RegisterDefeat();
        defeatPresenter.Open();
    }
}