public class ExternalSupportLinkOpener
{
    public void Open(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        UnityEngine.Application.OpenURL(url);
    }
}