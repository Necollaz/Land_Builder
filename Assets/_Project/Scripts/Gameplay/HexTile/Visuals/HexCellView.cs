using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HexCellView : MonoBehaviour
{
    [SerializeField] private GameObject _highlightObject;

    private void Awake()
    {
        _highlightObject.SetActive(false);
    }
        
    public void SetVisible(bool isVisible)
    {
        if (_highlightObject != null)
            _highlightObject.SetActive(isVisible);
    }
}