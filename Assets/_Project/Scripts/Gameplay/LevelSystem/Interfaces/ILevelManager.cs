using Cysharp.Threading.Tasks;
using System.Threading;

public interface ILevelManager
{
    public UniTask LoadLevel(LevelConfig config, CancellationToken token);
    public UniTask UnloadLevel(CancellationToken token);
    public LevelConfig CurrentLevel { get; }
}