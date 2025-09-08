using System.Collections.Generic;

public class ProgressService : IProgressService
{
    private readonly HashSet<int> _unlocked = new() { 1 };

    public bool IsLevelUnlocked(int id) => _unlocked.Contains(id);
    public void UnlockLevel(int id) => _unlocked.Add(id);
}