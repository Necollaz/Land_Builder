using System.Threading.Tasks;

public interface ILevelLoader
{
    public Task LoadLevel(LevelConfig config);
    public Task UnloadLevel();
}