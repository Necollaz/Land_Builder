using System.Threading.Tasks;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    
    public async Task Show()
    {
        if (_view != null)
            _view.SetActive(true);

        await Task.Yield();
    }
    
    public async Task Hide()
    {
        if (_view != null)
            _view.SetActive(false);

        await Task.Yield();
    }
}