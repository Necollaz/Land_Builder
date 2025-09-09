using UnityEngine;
using UnityEngine.UI;

public class UiToggleStateIcons : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _onIcon;
    [SerializeField] private Image _offIcon;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();

        if (_toggle == null)
            throw new MissingComponentException("UiToggleStateIcons: Toggle component is missing.");

        _toggle.onValueChanged.AddListener(ApplyState);
        
        ApplyState(_toggle.isOn);
    }

    private void OnDestroy()
    {
        if (_toggle != null)
            _toggle.onValueChanged.RemoveListener(ApplyState);
    }
    
    public void ForceRefreshFromValueWithoutNotify(bool isOn) => ApplyState(isOn);
    
    private void ApplyState(bool isOn)
    {
        if (_onIcon != null)
            _onIcon.gameObject.SetActive(isOn);
        
        if (_offIcon != null)
            _offIcon.gameObject.SetActive(!isOn);
    }
    
    private void Reset()
    {
        _toggle = GetComponent<Toggle>();
    }
}