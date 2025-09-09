using Zenject;

public class PauseSettingsPanel
{
    private readonly GamePauseSettingsModel gamePauseSettingsModel;
    private readonly PlayerPrefsSettingsStorage settingsStorage;
    private readonly ApplicationFramerateApplier framerateApplier;
    private readonly ExternalSupportLinkOpener supportLinkOpener;
    private readonly SceneNavigator sceneNavigator;
    
    private readonly string supportUrl;

    public PauseSettingsPanel(GamePauseSettingsModel gamePauseSettingsModel, PlayerPrefsSettingsStorage settingsStorage, ApplicationFramerateApplier framerateApplier,
        ExternalSupportLinkOpener supportLinkOpener, SceneNavigator sceneNavigator, [Inject(Id = "SupportUrl")] string supportUrl)
    {
        this.gamePauseSettingsModel = gamePauseSettingsModel;
        this.settingsStorage = settingsStorage;
        this.framerateApplier = framerateApplier;
        this.supportLinkOpener = supportLinkOpener;
        this.sceneNavigator = sceneNavigator;
        this.supportUrl = supportUrl;
        
        framerateApplier.Apply(gamePauseSettingsModel.Use60Fps);
        
        this.gamePauseSettingsModel.MusicEnabledChanged += OnMusicEnabledChanged;
        this.gamePauseSettingsModel.SoundEnabledChanged += OnSoundEnabledChanged;
        this.gamePauseSettingsModel.VibrationEnabledChanged += OnVibrationEnabledChanged;
        this.gamePauseSettingsModel.Use60FpsChanged += OnUse60FpsChanged;
    }

    public (bool music, bool sound, bool vibration, bool use60Fps) GetCurrentValues() => (gamePauseSettingsModel.MusicEnabled, gamePauseSettingsModel.SoundEnabled, gamePauseSettingsModel.VibrationEnabled, gamePauseSettingsModel.Use60Fps);
    
    public void OnMusicToggleUiChanged(bool value) => gamePauseSettingsModel.SetMusicEnabled(value);

    public void OnSoundToggleUiChanged(bool value) => gamePauseSettingsModel.SetSoundEnabled(value);

    public void OnVibrationToggleUiChanged(bool value) => gamePauseSettingsModel.SetVibrationEnabled(value);

    public void OnFpsToggleUiChanged(bool use60Fps) => gamePauseSettingsModel.SetUse60Fps(use60Fps);

    public void OnSupportButtonClicked() => supportLinkOpener.Open(supportUrl);

    public void OnMapButtonClicked() => sceneNavigator.LoadMainMenu();
    
    public void OnRestartButtonClicked() => sceneNavigator.RestartCurrentLevel();
    
    private void OnMusicEnabledChanged(bool value) => settingsStorage.SaveBool(SettingsStorageKeys.MUSIC_ENABLED, value);

    private void OnSoundEnabledChanged(bool value) => settingsStorage.SaveBool(SettingsStorageKeys.SOUND_ENABLED, value);

    private void OnVibrationEnabledChanged(bool value) => settingsStorage.SaveBool(SettingsStorageKeys.VIBRATION_ENABLED, value);

    private void OnUse60FpsChanged(bool use60Fps)
    {
        settingsStorage.SaveBool(SettingsStorageKeys.USE_60_FPS, use60Fps);
        framerateApplier.Apply(use60Fps);
    }
}