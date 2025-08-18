using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseDragPanInput : ICameraMoverInputProvider, ITickable
{
    public event Action<Vector2> PanInputed;
    
    private readonly int mouseButton;
    private readonly float effectiveSensitivity;
    private readonly bool ignoreWhenOverUI;
    
    private Vector2 _lastPosition;
    private bool _isDragging;

    public MouseDragPanInput(float sensitivity, bool invert = false, bool ignoreWhenOverUI = true, int mouseButton = 1)
    {
        this.mouseButton = mouseButton;
        this.ignoreWhenOverUI = ignoreWhenOverUI;
        this.effectiveSensitivity = invert ? -Mathf.Abs(sensitivity) : Mathf.Abs(sensitivity);
    }

    void ITickable.Tick()
    {
        if (ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(mouseButton))
        {
            _isDragging = true;
            _lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(mouseButton))
            _isDragging = false;

        if (!_isDragging)
            return;

        Vector2 currentPosition = Input.mousePosition;
        Vector2 deltaPixels = currentPosition - _lastPosition;
        Vector2 delta = deltaPixels * effectiveSensitivity;
        
        _lastPosition = currentPosition;

        if (delta != Vector2.zero)
            PanInputed?.Invoke(delta);
    }
}