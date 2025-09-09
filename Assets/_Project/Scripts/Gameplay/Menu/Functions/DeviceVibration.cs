public class DeviceVibration
{
    public void TryVibrate(bool vibrationEnabled)
    {
#if UNITY_IOS || UNITY_ANDROID
        if (vibrationEnabled)
            UnityEngine.Handheld.Vibrate();
#endif
    }
}