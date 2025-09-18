public class VictoryScreenPresenter
{
    private readonly VictoryRewardModel rewardModel;
    private readonly IRewardedAdService rewardedAdService;
    private readonly ICurrencyWallet currencyWallet;
    private readonly SceneNavigator sceneNavigator;
    private readonly VictoryScreenConfig config;

    private VictoryScreenView _view;

    public VictoryScreenPresenter(VictoryRewardModel rewardModel, IRewardedAdService rewardedAdService, ICurrencyWallet currencyWallet,
        SceneNavigator sceneNavigator, VictoryScreenConfig config)
    {
        this.rewardModel = rewardModel;
        this.rewardedAdService = rewardedAdService;
        this.currencyWallet = currencyWallet;
        this.sceneNavigator = sceneNavigator;
        this.config = config;

        this.rewardModel.RewardDoubled += OnRewardDoubled;
    }

    public void Attach(VictoryScreenView view)
    {
        _view = view;
        _view.SetCoinsText(rewardModel.FinalCoins);
        _view.SetDoubleButtonInteractable(!rewardModel.Doubled);
    }

    public void Detach()
    {
        _view = null;
    }

    public void OpenWithCoins(int baseCoins)
    {
        rewardModel.SetBaseCoins(baseCoins);
        
        if (_view != null)
        {
            _view.SetCoinsText(rewardModel.FinalCoins);
            _view.SetDoubleButtonInteractable(true);
            _view.Show();
        }
    }

    public void OnNextClicked()
    {
        currencyWallet.AddCoins(rewardModel.FinalCoins);
        
        if (_view != null)
            _view.Hide();

        if (config.GoToMainMenuOnNext)
            sceneNavigator.LoadMainMenu();
        else
            sceneNavigator.RestartCurrentLevel();
    }

    public void OnDoubleClicked()
    {
        if (_view != null)
            _view.SetDoubleButtonInteractable(false);
        
        rewardedAdService.ShowRewardedAd(success =>
        {
            if (!success)
            {
                if (_view != null)
                    _view.SetDoubleButtonInteractable(true);
                
                return;
            }

            rewardModel.ApplyDouble();
        });
    }

    private void OnRewardDoubled(int newFinalCoins)
    {
        if (_view == null)
            return;
        
        _view.SetCoinsText(newFinalCoins);
        _view.SetDoubleButtonInteractable(false);
    }
}