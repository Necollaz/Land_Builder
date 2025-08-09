using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraRotatorController : IDisposable
{
    private CinemachineFreeLook _camera;
    private IRotatorInputProvider _input;

    [Inject]
    public CameraRotatorController(CinemachineFreeLook camera, IRotatorInputProvider input)
    {
        _camera = camera;
        _input = input;
        
        _camera.m_XAxis.m_InputAxisValue = 0f;

        _input.OnRotatorInput += ApplyOrbit;
        Debug.Log("Camera Rotator Controller");
    }

    private void ApplyOrbit(float deltaDegrees)
    {
        _camera.m_XAxis.Value += deltaDegrees;
        Debug.Log("22222");
    }

    public void Dispose()
    {
        _input.OnRotatorInput -= ApplyOrbit;
    }
}
