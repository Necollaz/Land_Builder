using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraRotatorController : IDisposable
{
    private IRotatorInputProvider _input;
    private Transform _cameraTarget;

    [Inject]
    public CameraRotatorController(Transform cameraTarget, IRotatorInputProvider input)
    {
        _input = input;
        _cameraTarget = cameraTarget;

        _input.OnRotatorInput += ApplyOrbit;
    }

    private void ApplyOrbit(float deltaDegrees)
    {
        _cameraTarget.Rotate(deltaDegrees * Vector3.up * Time.deltaTime);
    }

    public void Dispose()
    {
        _input.OnRotatorInput -= ApplyOrbit;
    }
}
