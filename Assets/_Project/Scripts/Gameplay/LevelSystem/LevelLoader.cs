using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelLoader : ILevelLoader
{
    private readonly Score _score;
    private readonly HexGridInitializer _hexGridInitializer;

    private LevelConfig _currentLevel;
    private LoadingScreen _loadingScreen;

    public LevelLoader(Score score, HexGridInitializer gridInitializer, LoadingScreen loadingScreen)
    {
        _score = score;
        _hexGridInitializer = gridInitializer;
        _loadingScreen = loadingScreen;
    }

    public async UniTask LoadLevel(LevelConfig config)
    {
        _currentLevel = config;
        
        await _loadingScreen.Show();

        _score.SetValueForWin(config.RequiredScore);
        _score.Value.Value = 0;
        
        /*for (int i = 0; i < config.StartHexagons; i++)
        {
            
        }*/
        
        await Task.Delay(1000);

        await _loadingScreen.Hide();
    }

    public async UniTask UnloadLevel()
    {
        await UniTask.Yield();//сделать сброс текущих гексагонов
    }
}