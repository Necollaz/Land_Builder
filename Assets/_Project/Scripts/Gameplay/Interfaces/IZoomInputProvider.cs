using System;

public interface IZoomInputProvider
{
    public event Action<float> OnZoomInput;
}
