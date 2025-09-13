using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseChordedLRDragYawInput : ITickable, IRotatorInputProvider
{
    private const int LEFT_BUTTON = 0;
    private const int RIGHT_BUTTON = 1;
    
    public event Action<float> RotatorInputed;

    private readonly CameraSettingsConfig settings;
    private readonly float deadZoneUnits;
    private readonly float smoothingLerp01;
    private readonly float maxDegreesPerFrame;
    private readonly bool ignoreWhenOverUI;

    private float _filteredAxisX;
    private bool _chordActive;

    public MouseChordedLRDragYawInput(CameraSettingsConfig settings, bool ignoreWhenOverUI = true, float deadZoneUnits = 0.0f,
        float smoothingLerp01 = 0.35f, float maxDegreesPerFrame = 45f)
    {
        this.settings = settings;
        this.ignoreWhenOverUI = ignoreWhenOverUI;
        this.deadZoneUnits = Mathf.Max(0f, deadZoneUnits);
        this.smoothingLerp01 = Mathf.Clamp01(smoothingLerp01);
        this.maxDegreesPerFrame = Mathf.Max(0f, maxDegreesPerFrame);
    }

    void ITickable.Tick()
    {
        if (ignoreWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            _chordActive = false;
            _filteredAxisX = 0f;
            
            return;
        }

        bool left = Input.GetMouseButton(LEFT_BUTTON);
        bool right = Input.GetMouseButton(RIGHT_BUTTON);
        bool chord = left && right;
        
        if (!chord)
        {
            _chordActive = false;
            _filteredAxisX = 0f;
            
            return;
        }

        if (!_chordActive)
        {
            _chordActive = true;
            _filteredAxisX = 0f;
        }

        float rawAxisX = Input.GetAxisRaw("Mouse X");
        _filteredAxisX = Mathf.Lerp(_filteredAxisX, rawAxisX, smoothingLerp01);

        if (Mathf.Abs(_filteredAxisX) <= deadZoneUnits)
            return;

        float degrees = _filteredAxisX * settings.DesktopRotateSensitivity;
        
        if (maxDegreesPerFrame > 0f)
            degrees = Mathf.Clamp(degrees, -maxDegreesPerFrame, maxDegreesPerFrame);

        if (Mathf.Abs(degrees) > Mathf.Epsilon)
            RotatorInputed?.Invoke(degrees);
    }
}