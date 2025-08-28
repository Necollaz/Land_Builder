using System;

public class RotateInputAggregator : IRotatorInputProvider
{
    public event Action<float> RotatorInputed;
    
    private readonly IRotatorInputProvider[] inputs;
    
    public RotateInputAggregator(params IRotatorInputProvider[] providers)
    {
        inputs = providers;
        
        foreach (IRotatorInputProvider provider in providers)
        {
            if (provider == null) 
                continue;
            
            provider.RotatorInputed += d => RotatorInputed?.Invoke(d);
        }
    }
}