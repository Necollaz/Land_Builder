using Cysharp.Threading.Tasks;

public interface ILevelLoader
{
    public UniTask LoadLevel(LevelConfig config);
    public UniTask UnloadLevel();
}