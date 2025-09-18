using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HexagonPart : MonoBehaviour
{
    [SerializeField] private HexagonSides _side;
    [SerializeField] private HexType _type;
    [SerializeField] private HexTypePaletteConfig _palette;
    
    private Renderer _renderer;
    private MaterialPropertyBlock _materialProperties;

    public HexagonSides Side => _side;
    public HexType Type => _type;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _materialProperties = new MaterialPropertyBlock();
        
        ApplyColor(_type);
    }

    public void ApplyType(HexType type)
    {
        _type = type;
        
        ApplyColor(_type);
    }

    private void ApplyColor(HexType type)
    {
        if (_palette == null)
            return;
        
        _renderer.GetPropertyBlock(_materialProperties);
        _materialProperties.SetColor("_BaseColor", _palette.Get(type)); 
        _renderer.SetPropertyBlock(_materialProperties);
    }
}