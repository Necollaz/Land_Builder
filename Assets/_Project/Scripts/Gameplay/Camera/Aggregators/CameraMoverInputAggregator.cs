using System;
using UnityEngine;

public class CameraMoverInputAggregator : ICameraMoverInputProvider
{
    private ICameraMoverInputProvider[] _inputs;
    
    public event Action<Vector2> OnPanInput;

    public CameraMoverInputAggregator(params ICameraMoverInputProvider[] providers)
    {
        foreach (var p in providers)
        {
            if (p == null) 
                continue;
            
            p.OnPanInput += d => OnPanInput?.Invoke(d);
        }
    }

}