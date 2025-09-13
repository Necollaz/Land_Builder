using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DefeatScreenView : MonoBehaviour
{
    [Header("Roots")]
    [SerializeField] private GameObject _panelRoot;

    [Header("UI")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _extraTilesButton;

    private DefeatScreenPresenter _presenter;
    
    private bool _injected;
    private bool _initialized;

    [Inject]
    public void Construct(DefeatScreenPresenter presenter)
    {
        _presenter = presenter;
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
        
        if (_restartButton != null)
            _restartButton.onClick.RemoveListener(_presenter.OnRestartClicked);
        
        if (_extraTilesButton != null)
            _extraTilesButton.onClick.RemoveListener(_presenter.OnExtraClicked);
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

    public void SetExtraButtonVisible(bool visible)
    {
        if (_extraTilesButton != null && _extraTilesButton.gameObject.activeSelf != visible)
            _extraTilesButton.gameObject.SetActive(visible);
    }

    public void SetExtraButtonInteractable(bool interactable)
    {
        if (_extraTilesButton != null && _extraTilesButton.interactable != interactable)
            _extraTilesButton.interactable = interactable;
    }
    
    private void InitializeIfNeeded()
    {
        if (_initialized)
            return;

        if (_panelRoot != null)
            _panelRoot.SetActive(false);

        if (_restartButton != null)
            _restartButton.onClick.AddListener(_presenter.OnRestartClicked);

        if (_extraTilesButton != null)
            _extraTilesButton.onClick.AddListener(_presenter.OnExtraClicked);

        _initialized = true;
    }
}