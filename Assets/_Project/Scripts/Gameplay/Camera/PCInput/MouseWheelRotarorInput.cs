using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseWheelRotarorInput : ITickable, IRotatorInputProvider
{
    private float _sensitivity;
    private bool _invert = false;
    private bool _ignoreWhenOverUI = true;
    private bool _isDragging;
    private int _middleMouseButton = 2;

    public event Action<float> OnRotatorInput;

    public MouseWheelRotarorInput(float sensitivity)
    {
        _sensitivity = sensitivity;
    }
    
    public void Tick()
    {
        if (_ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(_middleMouseButton)) 
            _isDragging = true;
        
        if (Input.GetMouseButtonUp(_middleMouseButton)) 
            _isDragging = false;

        if (!_isDragging) 
            return;

        float dx = Input.GetAxisRaw("Mouse X");
        
        if (Mathf.Abs(dx) < Mathf.Epsilon) 
            return;

        float sign = _invert ? -1f : 1f;
        float delta = dx * _sensitivity * sign;
        
        OnRotatorInput?.Invoke(delta);
    }
}