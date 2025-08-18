using System;
using UnityEngine;
using Zenject;

public class TouchPinchOnlyZoomInput : ITickable, IZoomInputProvider
{
    private const int MOUSE_LEFT_BUTTON = 0;
    private const int MOUSE_RIGHT_BUTTON = 1;
    
    public event Action<float> ZoomInputed;
    
    private readonly float pinchSensitivity;
    
    private float? _previousDistance = null;

    [Inject]
    public TouchPinchOnlyZoomInput(float pinchSensitivity)
    {
        this.pinchSensitivity = pinchSensitivity;
    }
    
    void ITickable.Tick()
    {
        if (Input.touchCount < 2)
        {
            _previousDistance = null;
            
            return;
        }

        Touch touchMouseLeftButton = Input.GetTouch(MOUSE_LEFT_BUTTON);
        Touch touchMouseRightButton = Input.GetTouch(MOUSE_RIGHT_BUTTON);

        float currentDistance = Vector2.Distance(touchMouseLeftButton.position, touchMouseRightButton.position);

        if (_previousDistance.HasValue)
        {
            float delta = currentDistance - _previousDistance.Value;
            
            if (Mathf.Abs(delta) > 0.5f)
            {
                float zoomDelta = delta * pinchSensitivity;
                
                ZoomInputed?.Invoke(zoomDelta);
            }
        }

        _previousDistance = currentDistance;
    }
}