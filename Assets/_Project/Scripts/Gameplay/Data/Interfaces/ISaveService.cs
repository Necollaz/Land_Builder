public interface ISaveService
{
    public SaveData Load();
    
    public void Save(SaveData data);
    
    public string SaveFilePath { get; }
}