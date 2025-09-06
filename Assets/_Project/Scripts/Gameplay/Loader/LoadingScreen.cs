using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
}