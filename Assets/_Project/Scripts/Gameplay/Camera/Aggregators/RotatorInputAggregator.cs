using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorInputAggregator : IRotatorInputProvider
{
    public event Action<float> OnRotatorInput;
    
    public RotatorInputAggregator(params IRotatorInputProvider[] providers)
    {
        foreach (var p in providers)
        {
            if (p == null) continue;
            p.OnRotatorInput += d => OnRotatorInput?.Invoke(d);
        }
    }
}
