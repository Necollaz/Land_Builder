public class ApplicationFramerateApplier
{
    private const int FPS_30 = 30;
    private const int FPS_60 = 60;

    public void Apply(bool use60Fps)
    {
        UnityEngine.QualitySettings.vSyncCount = 0;
        UnityEngine.Application.targetFrameRate = use60Fps ? FPS_60 : FPS_30;
    }
}