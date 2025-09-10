public interface IProgressService
{
    public bool IsLevelUnlocked(int id);
    public void UnlockLevel(int id);
}