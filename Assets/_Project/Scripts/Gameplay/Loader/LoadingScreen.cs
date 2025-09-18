using System.Threading.Tasks;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view; // сам экран (панель) на Canvas

    /// <summary>Показать загрузочный экран</summary>
    public async Task Show()
    {
        if (_view != null)
            _view.SetActive(true);

        await Task.Yield(); // чтобы можно было вызывать как async
    }

    /// <summary>Скрыть загрузочный экран</summary>
    public async Task Hide()
    {
        if (_view != null)
            _view.SetActive(false);

        await Task.Yield();
    }
}