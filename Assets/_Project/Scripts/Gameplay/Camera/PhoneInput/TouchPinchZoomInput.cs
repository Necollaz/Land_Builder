using System;
using UnityEngine;
using Zenject;

public class TouchPinchZoomInput : ITickable, IZoomInputProvider
{
    private float _pinchSensitivity;
    private float? _previousDistance = null;
    
    public event Action<float> OnZoomInput;

    [Inject]
    public TouchPinchZoomInput(float pinchSensitivity)
    {
        _pinchSensitivity = pinchSensitivity;
    }
    
    public void Tick()
    {
        if (Input.touchCount < 2)
        {
            _previousDistance = null;
            return;
        }

        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        float currentDistance = Vector2.Distance(t0.position, t1.position);

        if (_previousDistance.HasValue)
        {
            float delta = currentDistance - _previousDistance.Value;
            if (Mathf.Abs(delta) > 0.5f)
            {
                float zoomDelta = delta * _pinchSensitivity;
                OnZoomInput?.Invoke(zoomDelta);
            }
        }

        _previousDistance = currentDistance;
    }
}