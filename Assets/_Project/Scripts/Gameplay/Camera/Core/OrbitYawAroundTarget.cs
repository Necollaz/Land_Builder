using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class OrbitYawAroundTarget : ITickable, IDisposable
{
    private readonly CameraSettingsConfig settings;
    private readonly CinemachineVirtualCamera virtualCamera;
    private readonly Transform orbitTarget;
    private readonly IRotatorInputProvider input;
    
    private float _targetYawDegrees;
    private float _yawVelocity;
    
    [Inject]
    public OrbitYawAroundTarget(Transform orbitTarget, IRotatorInputProvider input, CinemachineVirtualCamera virtualCamera, CameraSettingsConfig settings)
    {
        this.input = input;
        this.orbitTarget = orbitTarget;
        this.virtualCamera = virtualCamera;
        this.settings = settings;
        
        _targetYawDegrees = orbitTarget.eulerAngles.y;
        _yawVelocity = 0f;
        
        this.input.RotatorInputed += QueueOrbitDelta;
    }

    void ITickable.Tick()
    {
        float currentYaw = orbitTarget.eulerAngles.y;
        float maxSpeed = (settings.MaxRotateDegreesPerSecond <= 0f) ? Mathf.Infinity : settings.MaxRotateDegreesPerSecond;
        float smoothTime = Mathf.Max(0.001f, settings.RotateSmoothSeconds);
        float nextYaw = Mathf.SmoothDampAngle(currentYaw, _targetYawDegrees, ref _yawVelocity, smoothTime, maxSpeed, Time.deltaTime);
        
        Vector3 euler = orbitTarget.eulerAngles;
        euler.y = nextYaw;
        orbitTarget.rotation = Quaternion.Euler(euler);
    }
    
    void IDisposable.Dispose()
    {
        input.RotatorInputed -= QueueOrbitDelta;
    }
    
    private void QueueOrbitDelta(float deltaDegrees)
    {
        float scaled = deltaDegrees * GetRotateZoomScale();
        _targetYawDegrees += scaled;
        
        if (_targetYawDegrees > 360f || _targetYawDegrees < -360f)
            _targetYawDegrees = Mathf.Repeat(_targetYawDegrees, 360f);
    }
    
    private float GetRotateZoomScale()
    {
        if (virtualCamera == null || !settings.ScaleRotateByZoom)
            return 1f;

        float refSize = Mathf.Max(0.001f, settings.ReferenceOrthoSize);
        float size = virtualCamera.m_Lens.OrthographicSize;
        
        return refSize / size;
    }
}