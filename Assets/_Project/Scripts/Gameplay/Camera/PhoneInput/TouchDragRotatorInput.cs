using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TouchDragRotatorInput : ITickable, IRotatorInputProvider
{
    private float _sensitivity;
    private bool _invert = false;
    private bool _ignoreWhenOverUI = true;

    public event Action<float> OnRotatorInput;

    public TouchDragRotatorInput(float sensitivity)
    {
        _sensitivity = sensitivity;
    }
    
    public void Tick()
    {
        if (Input.touchCount != 1) 
            return;

        Touch touch = Input.GetTouch(0);

        if (_ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (touch.phase == TouchPhase.Moved)
        {
            float sign = _invert ? -1f : 1f;
            float deltaDegrees = touch.deltaPosition.x * _sensitivity * sign;
            
            if (Mathf.Abs(deltaDegrees) > 0f)
                OnRotatorInput?.Invoke(deltaDegrees);
        }
    }
}