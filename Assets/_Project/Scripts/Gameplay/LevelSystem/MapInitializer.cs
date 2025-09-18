public class MapInitializer
{
    private readonly LevelMapController _map;
    private readonly LevelConfig[] _levels;

    public MapInitializer(LevelMapController map, LevelConfig[] levels)
    {
        _map = map;
        _levels = levels;
        
        _map.BuildMap(_levels);
    }
}
