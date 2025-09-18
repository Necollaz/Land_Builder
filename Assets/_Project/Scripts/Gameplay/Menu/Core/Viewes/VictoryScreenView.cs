using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class VictoryScreenView : MonoBehaviour
{
    [Header("Roots")]
    [SerializeField] private GameObject _panelRoot;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _numberCoinsEarnedText;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _doubleRewardButton;

    [Header("Background Rotation")]
    [SerializeField] private WorldAutoRotator _worldAutoRotator;

    private VictoryScreenPresenter _presenter;
    private VictoryScreenConfig _config;
    
    private bool _injected;
    private bool _initialized;

    [Inject]
    public void Construct(VictoryScreenPresenter presenter, VictoryScreenConfig config)
    {
        _presenter = presenter;
        _config = config;
        
        _injected = true;

        InitializeIfNeeded();
    }
    
    private void OnEnable()
    {
        if (_injected)
            _presenter.Attach(this);
    }

    private void OnDisable()
    {
        if (_injected)
            _presenter.Detach();
    }
    
    private void OnDestroy()
    {
        if (!_initialized)
            return;
        
        if (_nextButton != null)
            _nextButton.onClick.RemoveListener(_presenter.OnNextClicked);
        
        if (_doubleRewardButton != null)
            _doubleRewardButton.onClick.RemoveListener(_presenter.OnDoubleClicked);
    }
    
    public void Show()
    {
        if (_panelRoot != null)
            _panelRoot.SetActive(true);
    }

    public void Hide()
    {
        if (_panelRoot != null)
            _panelRoot.SetActive(false);
    }

    public void SetCoinsText(int coins)
    {
        if (_numberCoinsEarnedText != null)
            _numberCoinsEarnedText.text = coins.ToString();
    }

    public void SetDoubleButtonInteractable(bool interactable)
    {
        if (_doubleRewardButton != null && _doubleRewardButton.interactable != interactable)
            _doubleRewardButton.interactable = interactable;
    }
    
    private void InitializeIfNeeded()
    {
        if (_initialized)
            return;

        if (_panelRoot != null)
            _panelRoot.SetActive(false);

        if (_worldAutoRotator != null)
            _worldAutoRotator.SetSpeed(_config.WorldRotationSpeedDegreesPerSecond);

        if (_nextButton != null)
            _nextButton.onClick.AddListener(_presenter.OnNextClicked);

        if (_doubleRewardButton != null)
            _doubleRewardButton.onClick.AddListener(_presenter.OnDoubleClicked);

        _initialized = true;
    }
}