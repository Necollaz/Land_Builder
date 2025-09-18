using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseHoldDragZoomInput : ITickable, IZoomInputProvider
{
    public event Action<float> ZoomInputed;
    
    private readonly int mouseButton = 0;
    private readonly float sensitivity;
    private readonly bool ignoreWhenOverUI;

    private Vector2 _lastPosition;
    private bool _isDragging;

    public MouseHoldDragZoomInput(float sensitivity, bool ignoreWhenOverUI = true)
    {
        this.sensitivity = sensitivity;
        this.ignoreWhenOverUI = ignoreWhenOverUI;
    }
    
    void ITickable.Tick()
    {
        if (ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        bool chord = Input.GetMouseButton(0) && Input.GetMouseButton(1);
        
        if (chord)
        {
            _isDragging = false;
            
            return;
        }
        
        if (Input.GetMouseButtonDown(mouseButton))
        {
            _isDragging = true;
            _lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(mouseButton))
            _isDragging = false;

        if (!_isDragging)
            return;

        Vector2 current = Input.mousePosition;
        Vector2 delta = current - _lastPosition;
        _lastPosition = current;

        if (Mathf.Abs(delta.y) > Mathf.Epsilon)
            ZoomInputed?.Invoke(delta.y * sensitivity);
    }
}