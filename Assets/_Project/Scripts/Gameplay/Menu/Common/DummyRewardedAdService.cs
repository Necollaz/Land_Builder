public class DummyRewardedAdService : IRewardedAdService
{
    public void ShowRewardedAd(System.Action<bool> onCompleted) => onCompleted?.Invoke(true);
}