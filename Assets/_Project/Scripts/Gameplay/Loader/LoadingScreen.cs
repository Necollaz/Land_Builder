using Cysharp.Threading.Tasks;
using UnityEngine;

public static class LoadingScreen
{
    private static GameObject _view;

    public static async UniTask Show()
    {
        if (_view == null)
            _view = Object.Instantiate(Resources.Load<GameObject>("LoadingScreen"));

        _view.SetActive(true);

        await UniTask.Yield();
    }

    public static async UniTask Hide()
    {
        _view.SetActive(false);
        await UniTask.Yield();
    }
}