using System;

public interface IRotatorInputProvider
{
    public event Action<float> RotatorInputed;
}