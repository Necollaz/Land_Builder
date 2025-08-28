using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class OrthoZoomWithCinemachine : IDisposable
{
    private readonly CinemachineVirtualCamera virtualCamera;
    private readonly IZoomInputProvider input;

    private readonly float zoomSpeed;
    private readonly float minZoom;
    private readonly float maxZoom;

    [Inject]
    public OrthoZoomWithCinemachine(CinemachineVirtualCamera virtualCamera, IZoomInputProvider input, float zoomSpeed, float minZoom, float maxZoom)
    {
        this.virtualCamera = virtualCamera;
        this.input = input;
        this.zoomSpeed = zoomSpeed;
        this.minZoom = minZoom;
        this.maxZoom = maxZoom;

        this.input.ZoomInputed += ApplyZoom;
    }

    void IDisposable.Dispose()
    {
        input.ZoomInputed -= ApplyZoom;
    }
    
    private void ApplyZoom(float delta)
    {
        float size = virtualCamera.m_Lens.OrthographicSize;
        
        size = Mathf.Clamp(size - delta * zoomSpeed, minZoom, maxZoom);
        
        virtualCamera.m_Lens.OrthographicSize = size;
    }
}