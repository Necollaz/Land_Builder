using System;

public class ZoomAggregator : IZoomInputProvider
{
    public event Action<float> OnZoomInput;

    public ZoomAggregator(params IZoomInputProvider[] providers)
    {
        foreach (var p in providers)
        {
            if (p == null) 
                continue;
            
            p.OnZoomInput += d => OnZoomInput?.Invoke(d);
        }
    }
}