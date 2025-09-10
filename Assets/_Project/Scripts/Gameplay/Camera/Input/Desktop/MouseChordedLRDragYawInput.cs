using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseChordedLRDragYawInput : ITickable, IRotatorInputProvider
{
    private const int LEFT_BUTTON = 0;
    private const int RIGHT_BUTTON = 1;
    
    public event Action<float> RotatorInputed;

    private readonly float sensitivity;
    private readonly bool ignoreWhenOverUI;

    private Vector2 _lastMousePosition;
    private bool _isDragging;

    public MouseChordedLRDragYawInput(float sensitivity, bool ignoreWhenOverUI = true)
    {
        this.sensitivity = sensitivity;
        this.ignoreWhenOverUI = ignoreWhenOverUI;
    }

    void ITickable.Tick()
    {
        if (ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        bool left = Input.GetMouseButton(LEFT_BUTTON);
        bool right = Input.GetMouseButton(RIGHT_BUTTON);
        bool chord = left && right;
        
        if (!_isDragging && chord)
        {
            _isDragging = true;
            _lastMousePosition = Input.mousePosition;
        }
        
        if (_isDragging && !chord)
        {
            _isDragging = false;
            
            return;
        }

        if (!_isDragging)
            return;

        Vector2 current = Input.mousePosition;
        Vector2 delta = current - _lastMousePosition;
        _lastMousePosition = current;

        if (Mathf.Abs(delta.x) > Mathf.Epsilon)
            RotatorInputed?.Invoke(delta.x * sensitivity);
    }
}