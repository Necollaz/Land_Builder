using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private LevelConfig _config;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _lockIcon;

    private LevelSelectionController _selection;
    private IProgressService _progress;

    [Inject]
    public void Construct(LevelSelectionController selection, IProgressService progress)
    {
        _selection = selection;
        _progress = progress;
    }

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
        Refresh();
    }

    public void Refresh()
    {
        bool unlocked = _progress.IsLevelUnlocked(_config.LevelId);
        _button.interactable = unlocked;
        if (_lockIcon != null) _lockIcon.SetActive(!unlocked);
    }

    private void OnClick()
    {
        _selection.SelectLevel(_config).Forget();
    }
}