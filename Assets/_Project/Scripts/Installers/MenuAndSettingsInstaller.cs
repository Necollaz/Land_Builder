using UnityEngine;
using Zenject;

public class MenuAndSettingsInstaller : MonoInstaller
{
    [SerializeField] private string _supportUrl = "https://example.com/support";
    
    [Header("Scene Views (optional)")]
    [SerializeField] private PauseSettingsPanelView _pauseSettingsView;
    
    private readonly bool defaultMusicEnabled = true;
    private readonly bool defaultSoundEnabled = true;
    private readonly bool defaultVibrationEnabled = true;
    private readonly bool defaultUse60Fps = false;

    public override void InstallBindings()
    {
        Container.Bind<PlayerPrefsSettingsStorage>().AsSingle();
        Container.Bind<GamePauseSettingsModel>().FromMethod(context =>
            {
                var storage = context.Container.Resolve<PlayerPrefsSettingsStorage>();

                bool music = TryLoadOrDefault(storage, SettingsStorageKeys.MUSIC_ENABLED, true);
                bool sound = TryLoadOrDefault(storage, SettingsStorageKeys.SOUND_ENABLED, true);
                bool vibration = TryLoadOrDefault(storage, SettingsStorageKeys.VIBRATION_ENABLED, true);
                bool use60 = TryLoadOrDefault(storage, SettingsStorageKeys.USE_60_FPS, false);

                return new GamePauseSettingsModel(music, sound, vibration, use60);
            })
            .AsSingle();
        
        Container.Bind<ApplicationFramerateApplier>().AsSingle();
        Container.Bind<ExternalSupportLinkOpener>().AsSingle();
        Container.Bind<string>().WithId("SupportUrl").FromInstance(_supportUrl).AsSingle();
        Container.Bind<PauseSettingsPanel>().AsSingle();
        
        if (_pauseSettingsView != null)
            Container.QueueForInject(_pauseSettingsView);
    }

    private bool TryLoadOrDefault(PlayerPrefsSettingsStorage storage, string key, bool defaultValue)
    {
        if (storage.TryLoadBool(key, out bool value))
            return value;

        storage.SaveBool(key, defaultValue);
        
        return defaultValue;
    }
}