using System;
using UnityEngine;
using Zenject;

public class MouseWheelZoomInput : ITickable, IZoomInputProvider
{
    public event Action<float> ZoomInputed;
    
    private readonly float zoomSensitivity;

    public MouseWheelZoomInput(float zoomSensitivity)
    {
        this.zoomSensitivity = zoomSensitivity;
    }
    
    void ITickable.Tick()
    {
        float wheel = Input.mouseScrollDelta.y;
        
        if (Mathf.Abs(wheel) > 0f)
            ZoomInputed?.Invoke(wheel * zoomSensitivity);
    }
}