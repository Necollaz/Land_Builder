using UnityEngine;

public class DefeatScreenPresenter
{
    private readonly DefeatAttemptModel defeatAttemptModel;
    private readonly IRewardedAdService rewardedAdService;
    private readonly IExtraTilesGranter extraTilesGranter;
    private readonly SceneNavigator sceneNavigator;
    private readonly DefeatScreenConfig config;

    private DefeatScreenView _view;

    public DefeatScreenPresenter(DefeatAttemptModel defeatAttemptModel, IRewardedAdService rewardedAdService, IExtraTilesGranter extraTilesGranter,
        SceneNavigator sceneNavigator, DefeatScreenConfig config)
    {
        this.defeatAttemptModel = defeatAttemptModel;
        this.rewardedAdService = rewardedAdService;
        this.extraTilesGranter = extraTilesGranter;
        this.sceneNavigator = sceneNavigator;
        this.config = config;
    }

    public void Attach(DefeatScreenView view)
    {
        _view = view;
        _view.SetExtraButtonVisible(defeatAttemptModel.CanOfferExtra);
        _view.SetExtraButtonInteractable(true);
    }

    public void Detach()
    {
        _view = null;
    }

    public void Open()
    {
        if (_view == null)
            return;
        
        _view.SetExtraButtonVisible(defeatAttemptModel.CanOfferExtra);
        _view.SetExtraButtonInteractable(true);
        _view.Show();
    }

    public void OnRestartClicked()
    {
        defeatAttemptModel.ResetSession();
        
        if (_view != null)
            _view.Hide();
        
        sceneNavigator.RestartCurrentLevel();
    }

    public void OnExtraClicked()
    {
        if (!defeatAttemptModel.CanOfferExtra)
            return;

        if (_view != null)
            _view.SetExtraButtonInteractable(false);

        rewardedAdService.ShowRewardedAd(success =>
        {
            if (!success)
            {
                if (_view != null)
                    _view.SetExtraButtonInteractable(true);
                
                return;
            }

            extraTilesGranter.GrantExtraTiles(config.ExtraTilesOnReward);
            defeatAttemptModel.MarkExtraGranted();

            if (_view != null)
            {
                _view.SetExtraButtonVisible(false);
                
                if (config.CloseScreenAfterGrant)
                    _view.Hide();
                else
                    _view.SetExtraButtonInteractable(false);
            }
        });
    }
}