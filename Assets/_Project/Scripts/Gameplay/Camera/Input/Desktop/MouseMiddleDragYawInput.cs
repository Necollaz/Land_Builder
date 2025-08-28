using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseMiddleDragYawInput : ITickable, IRotatorInputProvider
{
    private const string MOUSE_X = "Mouse X";
    private const int MOUSE_MIDDLE_BUTTON = 2;
    public event Action<float> RotatorInputed;
    
    private readonly float sensitivity;
    private readonly bool invert = false;
    private readonly bool ignoreWhenOverUI = true;
    
    private bool _isDragging;

    public MouseMiddleDragYawInput(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }
    
    void ITickable.Tick()
    {
        if (ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(MOUSE_MIDDLE_BUTTON)) 
            _isDragging = true;
        
        if (Input.GetMouseButtonUp(MOUSE_MIDDLE_BUTTON)) 
            _isDragging = false;

        if (!_isDragging) 
            return;

        float directionX = Input.GetAxisRaw(MOUSE_X);
        
        if (Mathf.Abs(directionX) < Mathf.Epsilon) 
            return;

        float sign = invert ? -1f : 1f;
        float delta = directionX * sensitivity * sign;
        
        RotatorInputed?.Invoke(delta);
    }
}