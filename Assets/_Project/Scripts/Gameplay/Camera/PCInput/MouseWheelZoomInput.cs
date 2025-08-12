using System;
using UnityEngine;
using Zenject;

public class MouseWheelZoomInput : ITickable, IZoomInputProvider
{
    private float _zoomSensitivity;
    
    public event Action<float> OnZoomInput;

    public MouseWheelZoomInput(float zoomSensitivity)
    {
        _zoomSensitivity = zoomSensitivity;
    }
    
    public void Tick()
    {
        float wheel = Input.mouseScrollDelta.y;
        
        if (Mathf.Abs(wheel) > 0f)
            OnZoomInput?.Invoke(wheel * _zoomSensitivity);
    }
}
