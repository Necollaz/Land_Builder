using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CameraMouseMover : ICameraMoverInputProvider, ITickable
{
    private float _sensitivity;
    private bool _invert;
    private bool _ignoreWhenOverUI;
    private int _mouseButton;
    private bool _isDragging;
    private Vector2 _lastPos;

    public event Action<Vector2> OnPanInput;

    public CameraMouseMover(float sensitivity, bool invert = false, bool ignoreWhenOverUI = true, int mouseButton = 1)
    {
        _sensitivity = sensitivity;
        _invert = invert;
        _ignoreWhenOverUI = ignoreWhenOverUI;
        _mouseButton = mouseButton;
    }

    public void Tick()
    {
        if (_ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(_mouseButton))
        {
            _isDragging = true;
            _lastPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(_mouseButton))
        {
            _isDragging = false;
        }

        if (!_isDragging) return;

        Vector2 currentPos = Input.mousePosition;
        Vector2 delta = (currentPos - _lastPos) * (_invert ? -1f : 1f) * _sensitivity;
        _lastPos = currentPos;

        if (delta != Vector2.zero)
            OnPanInput?.Invoke(delta);
    }
}
