using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TouchDragPanInput : ICameraMoverInputProvider, ITickable
{
    public event Action<Vector2> PanInputed;
    
    private readonly float effectiveSensitivity;
    private readonly bool ignoreWhenOverUI;

    public TouchDragPanInput(float sensitivity, bool invert = false, bool ignoreWhenOverUI = true)
    {
        this.ignoreWhenOverUI = ignoreWhenOverUI;
        this.effectiveSensitivity = invert ? -Mathf.Abs(sensitivity) : Mathf.Abs(sensitivity);
    }

    void ITickable.Tick()
    {
        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);

        if (ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 delta = touch.deltaPosition * effectiveSensitivity;
            
            PanInputed?.Invoke(delta);
        }
    }
}