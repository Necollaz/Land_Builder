using Cysharp.Threading.Tasks;

public class LevelSelectionController
{
    private readonly ILevelLoader _levelLoader;
    private readonly IProgressService _progress;

    public LevelSelectionController(ILevelLoader loader, IProgressService progress)
    {
        _levelLoader = loader;
        _progress = progress;
    }

    public async UniTask SelectLevel(LevelConfig config)
    {
        if (!_progress.IsLevelUnlocked(config.LevelId))
            return;

        await _levelLoader.LoadLevel(config);
    }

    public void CompleteLevel(LevelConfig config)
    {
        foreach (var next in config.NextLevels)
            _progress.UnlockLevel(next.LevelId);
    }
}