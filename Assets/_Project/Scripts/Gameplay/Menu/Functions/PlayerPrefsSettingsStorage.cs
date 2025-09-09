public class PlayerPrefsSettingsStorage
{
    private const int TRUE_INT = 1;
    private const int FALSE_INT = 0;

    public bool TryLoadBool(string key, out bool value)
    {
        if (!UnityEngine.PlayerPrefs.HasKey(key))
        {
            value = default;
            
            return false;
        }

        int stored = UnityEngine.PlayerPrefs.GetInt(key, FALSE_INT);
        value = stored == TRUE_INT;
        
        return true;
    }

    public void SaveBool(string key, bool value)
    {
        UnityEngine.PlayerPrefs.SetInt(key, value ? TRUE_INT : FALSE_INT);
        UnityEngine.PlayerPrefs.Save();
    }
}