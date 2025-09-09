using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings Defaults", fileName = "SettingsDefaults")]
public class SettingsDefaultsConfig : ScriptableObject
{
    [Header("Initial Toggles")]
    public bool MusicEnabledByDefault = true;
    public bool SoundEnabledByDefault = true;
    public bool VibrationEnabledByDefault = true;
    public bool Use60FpsByDefault = true;

    [Header("Support")]
    public string SupportUrl = "https://example.com/support";
}