using System.IO;
using UnityEngine;

public class SaveService : ISaveService
{
    private const string FILE_NAME = "savefile.json";
    
    public string SaveFilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);

    public SaveData Load()
    {
        try
        {
            if (!File.Exists(SaveFilePath))
                return new SaveData();

            var json = File.ReadAllText(SaveFilePath);
            
            if (string.IsNullOrEmpty(json))
                return new SaveData();

            return JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        }
        catch (System.Exception error)
        {
            Debug.LogError($"SaveService.Load error: {error}");
            
            return new SaveData();
        }
    }

    public void Save(SaveData data)
    {
        try
        {
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(SaveFilePath, json);
        }
        catch (System.Exception error)
        {
            Debug.LogError($"SaveService.Save error: {error}");
        }
    }
}