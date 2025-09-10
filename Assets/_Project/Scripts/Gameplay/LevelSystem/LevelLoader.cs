using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelLoader : ILevelLoader
{
    private readonly Score _score;
    private readonly HexGridInitializer _hexGridInitializer;

    private LevelConfig _currentLevel;

    public LevelLoader(Score score, HexGridInitializer gridInitializer)
    {
        _score = score;
        _hexGridInitializer = gridInitializer;
    }

    public async UniTask LoadLevel(LevelConfig config)
    {
        _currentLevel = config;
        
        await LoadingScreen.Show();

        _score.SetValueForWin(config.RequiredScore);
        _score.Value.Value = 0;
        
        /*for (int i = 0; i < config.StartHexagons; i++)
        {
            
        }*/
        
        await UniTask.Delay(1000);

        await LoadingScreen.Hide();
    }

    public async UniTask UnloadLevel()
    {
        await UniTask.Yield();//сделать сброс текущих гексагонов
    }
}