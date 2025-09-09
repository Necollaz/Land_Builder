public class GamePauseSettingsModel
{
    public delegate void BoolChanged(bool value);

    public event BoolChanged MusicEnabledChanged;
    public event BoolChanged SoundEnabledChanged;
    public event BoolChanged VibrationEnabledChanged;
    public event BoolChanged Use60FpsChanged;

    public bool MusicEnabled { get; private set; }
    public bool SoundEnabled { get; private set; }
    public bool VibrationEnabled { get; private set; }
    public bool Use60Fps { get; private set; }

    public GamePauseSettingsModel(bool musicEnabled, bool soundEnabled, bool vibrationEnabled, bool use60Fps)
    {
        MusicEnabled = musicEnabled;
        SoundEnabled = soundEnabled;
        VibrationEnabled = vibrationEnabled;
        Use60Fps = use60Fps;
    }

    public void SetMusicEnabled(bool enabled)
    {
        if (MusicEnabled == enabled)
            return;
        
        MusicEnabled = enabled;
        
        MusicEnabledChanged?.Invoke(MusicEnabled);
    }

    public void SetSoundEnabled(bool enabled)
    {
        if (SoundEnabled == enabled)
            return;
        
        SoundEnabled = enabled;
        
        SoundEnabledChanged?.Invoke(SoundEnabled);
    }

    public void SetVibrationEnabled(bool enabled)
    {
        if (VibrationEnabled == enabled)
            return;
        
        VibrationEnabled = enabled;
        
        VibrationEnabledChanged?.Invoke(VibrationEnabled);
    }

    public void SetUse60Fps(bool use60)
    {
        if (Use60Fps == use60)
            return;
        
        Use60Fps = use60;
        
        Use60FpsChanged?.Invoke(Use60Fps);
    }
}