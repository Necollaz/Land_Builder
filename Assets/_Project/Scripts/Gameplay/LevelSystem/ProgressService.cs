using System.Collections.Generic;
using System.Linq;

public class ProgressService : IProgressService
{
    private readonly ISaveService _saveService;
    private readonly HashSet<int> _unlocked = new();

    public ProgressService(ISaveService saveService)
    {
        _saveService = saveService;
        var data = _saveService.Load();
        
        if (data?.UnlockedLevels != null && data.UnlockedLevels.Count > 0)
            foreach (var id in data.UnlockedLevels)
                _unlocked.Add(id);
        else
            _unlocked.Add(1);
    }

    public bool IsLevelUnlocked(int id) => _unlocked.Contains(id);

    public void UnlockLevel(int id)
    {
        if (_unlocked.Add(id))
            Save();
    }

    private void Save()
    {
        var data = new SaveData { UnlockedLevels = _unlocked.ToList() };
        _saveService.Save(data);
    }
}