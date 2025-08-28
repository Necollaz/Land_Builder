using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HexRandomize : MonoBehaviour
{
    private readonly int SeedID = Shader.PropertyToID("_Seed");
    
    [SerializeField] private int _seedOverride = 0;

    private void Awake()
    {
        Renderer hexRenderer = GetComponent<Renderer>();
        MaterialPropertyBlock properties = new MaterialPropertyBlock();
        
        hexRenderer.GetPropertyBlock(properties);
        
        int seed = _seedOverride != 0 ? _seedOverride : (int)(Random.value * 1_000_000f);
        
        properties.SetFloat(SeedID, seed);
        hexRenderer.SetPropertyBlock(properties);
    }
}