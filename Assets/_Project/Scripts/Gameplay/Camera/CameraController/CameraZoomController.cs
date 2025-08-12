using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraZoomController : IDisposable
{
    private CinemachineVirtualCamera _camera;
    private IZoomInputProvider _input;

    private float _zoomSpeed;
    private float _minZoom;
    private float _maxZoom;

    [Inject]
    public CameraZoomController(CinemachineVirtualCamera camera, IZoomInputProvider input, float zoomSpeed, float minZoom, float maxZoom)
    {
        _camera = camera;
        _input = input;
        _zoomSpeed = zoomSpeed;
        _minZoom = minZoom;
        _maxZoom = maxZoom;

        _input.OnZoomInput += ApplyZoom;
    }

    private void ApplyZoom(float delta)
    {
        _camera.m_Lens.OrthographicSize = Mathf.Clamp(_camera.m_Lens.OrthographicSize - delta * _zoomSpeed, _minZoom, _maxZoom);
    }

    public void Dispose()
    {
        _input.OnZoomInput -= ApplyZoom;
    }
}