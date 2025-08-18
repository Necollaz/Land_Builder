using System;

public class ZoomInputAggregator : IZoomInputProvider
{
    public event Action<float> ZoomInputed;
    
    private readonly IZoomInputProvider[] inputs;

    public ZoomInputAggregator(params IZoomInputProvider[] providers)
    {
        inputs = providers;
        
        foreach (IZoomInputProvider provider in providers)
        {
            if (provider == null) 
                continue;
            
            provider.ZoomInputed += d => ZoomInputed?.Invoke(d);
        }
    }
}