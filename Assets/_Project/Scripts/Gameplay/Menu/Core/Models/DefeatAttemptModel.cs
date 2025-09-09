public class DefeatAttemptModel
{
    public int DefeatCount { get; private set; }
    public bool ExtraTilesGrantedOnce { get; private set; }

    public bool CanOfferExtra => DefeatCount == 1 && !ExtraTilesGrantedOnce;

    public void RegisterDefeat() => DefeatCount++;

    public void MarkExtraGranted() => ExtraTilesGrantedOnce = true;

    public void ResetSession()
    {
        DefeatCount = 0;
        ExtraTilesGrantedOnce = false;
    }
}