using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class OrbitYawAroundTarget : IDisposable
{
    private readonly CameraSettingsConfig settings;
    private readonly CinemachineVirtualCamera virtualCamera;
    private readonly Transform orbitTarget;
    private readonly IRotatorInputProvider input;
    
    [Inject]
    public OrbitYawAroundTarget(Transform orbitTarget, IRotatorInputProvider input, CinemachineVirtualCamera virtualCamera, CameraSettingsConfig settings)
    {
        this.input = input;
        this.orbitTarget = orbitTarget;
        this.virtualCamera = virtualCamera;
        this.settings = settings;
        
        this.input.RotatorInputed += ApplyOrbit;
    }

    void IDisposable.Dispose()
    {
        input.RotatorInputed -= ApplyOrbit;
    }
    
    private void ApplyOrbit(float deltaDegrees)
    {
        float scaled = deltaDegrees * GetRotateZoomScale();
        
        orbitTarget.Rotate(Vector3.up, scaled, Space.World);
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