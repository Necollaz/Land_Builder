using System.Threading;
using Cysharp.Threading.Tasks;

public class LevelManager : ILevelManager
{
    public LevelConfig CurrentLevel { get; private set; }

    /*private readonly IHexagonSpawner _hexSpawner;
    private readonly IScoreSystem _scoreSystem;
    private readonly IRewardSystem _rewardSystem;

    public LevelManager(IHexagonSpawner spawner, IScoreSystem scoreSystem, IRewardSystem rewardSystem)
    {
        _hexSpawner = spawner;
        _scoreSystem = scoreSystem;
        _rewardSystem = rewardSystem;
    }*/

    public async UniTask LoadLevel(LevelConfig config, CancellationToken token)
    {
        CurrentLevel = config;
        
        await UniTask.Delay(1000, cancellationToken: token);

        //_hexSpawner.SpawnHexagons(config.StartHexagons);
        //_scoreSystem.SetTargetScore(config.RequiredScore);
    }

    public async UniTask UnloadLevel(CancellationToken token)
    {
        await UniTask.Delay(500, cancellationToken: token);
        //_hexSpawner.Clear();
    }
}