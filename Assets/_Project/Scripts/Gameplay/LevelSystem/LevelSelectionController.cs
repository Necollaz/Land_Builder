using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] private LoadingScreen _loadingScreen;

    private ILevelManager _levelManager;

    [Inject]
    public void Construct(ILevelManager manager)
    {
        _levelManager = manager;
    }

    public async void SelectLevel(LevelConfig config)
    {
        _loadingScreen.Show();
        await _levelManager.UnloadLevel(this.GetCancellationTokenOnDestroy());
        await _levelManager.LoadLevel(config, this.GetCancellationTokenOnDestroy());
        _loadingScreen.Hide();
    }
}