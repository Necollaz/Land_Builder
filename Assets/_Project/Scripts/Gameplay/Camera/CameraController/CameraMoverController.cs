using System;
using UnityEngine;
using Zenject;

public class CameraMoverController : IDisposable
{
    private Transform _target;
    private Camera _camera;
    private ICameraMoverInputProvider _input;
    private float _panMultiplier;
    private float _minX;
    private float _maxX;
    private float _minZ;
    private float _maxZ;

    [Inject]
    public CameraMoverController(Transform target, Camera camera, ICameraMoverInputProvider input,
        float panMultiplier, float minX, float maxX, float minZ, float maxZ)
    {
        _target = target;
        _camera = camera;
        _input = input;
        _panMultiplier = panMultiplier;
        _minX = minX;
        _maxX = maxX;
        _minZ = minZ;
        _maxZ = maxZ;

        _input.OnPanInput += Pan;
    }

    private void Pan(Vector2 screenDelta)
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0f, _target.position.y, 0f));

        Ray preview = _camera.ScreenPointToRay(Input.mousePosition - (Vector3)screenDelta);
        Ray current = _camera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(preview, out float enterPreview) && plane.Raycast(current, out float enterCurrent))
        {
            Vector3 worldPreview = preview.GetPoint(enterPreview);
            Vector3 worldCurrent = current.GetPoint(enterCurrent);

            Vector3 desiredPosition = _target.position + (worldPreview - worldCurrent) * _panMultiplier;
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, _minX, _maxX);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, _minZ, _maxZ);
            desiredPosition.y = _target.position.y;

            _target.position = desiredPosition;
        }
    }

    public void Dispose()
    {
        _input.OnPanInput -= Pan;
    }
}