using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelNodeView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private GameObject _lockIcon;

    private LevelConfig _config;
    private Action<LevelConfig> _onSelected;
    
    public void Initialize(LevelConfig config, bool unlocked, Action<LevelConfig> onSelected)
    {
        _config = config;
        _onSelected = onSelected;

        _label.text = config.LevelId.ToString();
        _lockIcon.SetActive(!unlocked);

        _button.interactable = unlocked;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => _onSelected?.Invoke(_config));
    }
}