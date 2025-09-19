using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using UniRx;

public class LevelLoader : MonoBehaviour, ILevelLoader
{
    [Header("UI")]
    [SerializeField] private GameObject _levelSelectPanel;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;

    private Score _score;
    private HexTileGridBuilder _gridBuilder;
    private HexGridInitializer _initializer;
    private LoadingScreen _loadingScreen;
    private int _remainingHexagons;
    private LevelConfig _current;
    private LevelSelectionController _levelSelectionController;

    [Inject]
    public void Construct(
        Score score,
        HexTileGridBuilder gridBuilder,
        HexGridInitializer initializer,
        LoadingScreen loadingScreen,
        LevelSelectionController levelSelectionController)
    {
        _score = score;
        _gridBuilder = gridBuilder;
        _initializer = initializer;
        _loadingScreen = loadingScreen;
        _levelSelectionController = levelSelectionController;
    }

    public async Task LoadLevel(LevelConfig cfg)
    {
        _current = cfg;

        _levelSelectPanel.SetActive(false);
        await _loadingScreen.Show();

        _score.Value.Value = 0;
        _score.SetValueForWin(cfg.RequiredScore);
        _remainingHexagons = cfg.StartHexagons;

        _gridBuilder.Build(Vector2Int.zero);

        _gameplayUI.SetActive(true);
        await Task.Delay(500);
        await _loadingScreen.Hide();

        _score.OnValueEnd
            .Subscribe(_ => OnWin())
            .AddTo(this);
    }

    public Task UnloadLevel() => Task.CompletedTask;

    private void OnWin()
    {
        _levelSelectionController.CompleteLevel(_current);
        
        _winPanel.SetActive(true);
        _gameplayUI.SetActive(false);
    }

    public void OnHexagonPlaced()
    {
        _remainingHexagons--;
        if (_remainingHexagons <= 0 && _score.Value.Value < _current.RequiredScore)
        {
            _losePanel.SetActive(true);
            _gameplayUI.SetActive(false);
        }
    }
}