using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseSettingsPanelView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _panelRoot;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _vibrationToggle;
    [SerializeField] private Toggle _fps60Toggle;
    [SerializeField] private Button _supportButton;

    [Header("Open/Close")]
    [SerializeField] private Button _openSettingsButton;
    [SerializeField] private Button _closeSettingsButton;
    
    [Header("Scene Flow")]
    [SerializeField] private Button _mapButton;
    [SerializeField] private Button _restartButton;

    private PauseSettingsPanel _pauseSettings;
    
    private bool _injected;
    private bool _initialized;

    [Inject]
    public void Construct(PauseSettingsPanel pauseSettings)
    {
        _pauseSettings = pauseSettings;
        _injected = true;

        InitializeIfNeeded();
        HidePanel();
    }
    
    private void OnEnable()
    {
        if (!_injected)
            return;
        
        var (music, sound, vibration, use60) = _pauseSettings.GetCurrentValues();
        
        _musicToggle.SetIsOnWithoutNotify(music);
        _soundToggle.SetIsOnWithoutNotify(sound);
        _vibrationToggle.SetIsOnWithoutNotify(vibration);
        _fps60Toggle.SetIsOnWithoutNotify(use60);
        
        RefreshIconsAfterSilentSet();
    }

    private void OnDestroy()
    {
        if (!_initialized)
            return;
        
        _openSettingsButton.onClick.RemoveAllListeners();
        _closeSettingsButton.onClick.RemoveAllListeners();
        _supportButton.onClick.RemoveAllListeners();
        _musicToggle.onValueChanged.RemoveAllListeners();
        _soundToggle.onValueChanged.RemoveAllListeners();
        _vibrationToggle.onValueChanged.RemoveAllListeners();
        _fps60Toggle.onValueChanged.RemoveAllListeners();
        
        if (_mapButton != null)
            _mapButton.onClick.RemoveAllListeners();
        
        if (_restartButton != null)
            _restartButton.onClick.RemoveAllListeners();
    }
    
    private void InitializeIfNeeded()
    {
        if (_initialized)
            return;

        _openSettingsButton.onClick.AddListener(ShowPanel);
        _closeSettingsButton.onClick.AddListener(HidePanel);
        _supportButton.onClick.AddListener(() => _pauseSettings.OnSupportButtonClicked());
        _musicToggle.onValueChanged.AddListener(_pauseSettings.OnMusicToggleUiChanged);
        _soundToggle.onValueChanged.AddListener(_pauseSettings.OnSoundToggleUiChanged);
        _vibrationToggle.onValueChanged.AddListener(_pauseSettings.OnVibrationToggleUiChanged);
        _fps60Toggle.onValueChanged.AddListener(_pauseSettings.OnFpsToggleUiChanged);

        if (_mapButton != null)
            _mapButton.onClick.AddListener(() =>
            {
                HidePanel();
                _pauseSettings.OnMapButtonClicked();
            });

        if (_restartButton != null)
            _restartButton.onClick.AddListener(() =>
            {
                HidePanel();
                _pauseSettings.OnRestartButtonClicked();
            });

        _initialized = true;
    }
    
    private void ShowPanel() => _panelRoot.gameObject.SetActive(true);

    private void HidePanel() => _panelRoot.gameObject.SetActive(false);
    
    private void RefreshIconsAfterSilentSet()
    {
        TryRefreshIcons(_musicToggle, _musicToggle.isOn);
        TryRefreshIcons(_soundToggle, _soundToggle.isOn);
        TryRefreshIcons(_vibrationToggle, _vibrationToggle.isOn);
        TryRefreshIcons(_fps60Toggle, _fps60Toggle.isOn);
    }
    
    private void TryRefreshIcons(Toggle toggle, bool value)
    {
        if (toggle == null)
            return;
        
        if (toggle.TryGetComponent(out UiToggleStateIcons icons))
            if (icons != null)
                icons.ForceRefreshFromValueWithoutNotify(value);
    }
}