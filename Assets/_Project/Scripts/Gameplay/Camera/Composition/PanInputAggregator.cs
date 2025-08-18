using System;
using UnityEngine;

public class PanInputAggregator : ICameraMoverInputProvider
{
    private readonly ICameraMoverInputProvider[] inputs;
    
    public event Action<Vector2> PanInputed;

    public PanInputAggregator(params ICameraMoverInputProvider[] providers)
    {
        inputs = providers;
        
        foreach (ICameraMoverInputProvider provider in providers)
        {
            if (provider == null) 
                continue;
            
            provider.PanInputed += d => PanInputed?.Invoke(d);
        }
    }
}