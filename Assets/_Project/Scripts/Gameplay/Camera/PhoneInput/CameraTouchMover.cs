using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CameraTouchMover : ICameraMoverInputProvider, ITickable
{
    private float _sensitivity;
    private bool _invert;
    private bool _ignoreWhenOverUI;

    public event Action<Vector2> OnPanInput;

    public CameraTouchMover(float sensitivity, bool invert = false, bool ignoreWhenOverUI = true)
    {
        _sensitivity = sensitivity;
        _invert = invert;
        _ignoreWhenOverUI = ignoreWhenOverUI;
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
            Vector2 delta = touch.deltaPosition * (_invert ? -1f : 1f) * _sensitivity;
            OnPanInput?.Invoke(delta);
        }
    }
}