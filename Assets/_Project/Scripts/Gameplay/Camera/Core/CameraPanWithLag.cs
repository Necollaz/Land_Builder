using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraPanWithLag : ITickable, IDisposable
{
    private readonly Camera camera;
    private readonly CameraSettingsConfig settings;
    private readonly CinemachineVirtualCamera virtualCamera;
    private readonly Transform cameraTarget;
    private readonly ICameraMoverInputProvider input;
    
    private readonly float panWorldMultiplier;
    private readonly float minX;
    private readonly float maxX;
    private readonly float minZ;
    private readonly float maxZ;

    private readonly float panSmoothSeconds;
    private readonly float maxPanUnitsPerSecond;

    private Vector3 _desiredTargetPosition;
    private Vector3 _panVelocity;
    
    [Inject]
    public CameraPanWithLag(Transform cameraTarget, Camera camera, ICameraMoverInputProvider input, CinemachineVirtualCamera virtualCamera,
        CameraSettingsConfig settings)
    {
        this.cameraTarget = cameraTarget;
        this.camera = camera;
        this.input = input;
        this.virtualCamera = virtualCamera;
        this.settings = settings;

        _desiredTargetPosition = this.cameraTarget.position;
        
        this.input.PanInputed += Pan;
    }

    void IDisposable.Dispose()
    {
        input.PanInputed -= Pan;
    }
    
    void ITickable.Tick()
    {
        Vector3 clamped = ClampXZ(_desiredTargetPosition, cameraTarget.position.y);
        _desiredTargetPosition = clamped;

        float maxDelta = Mathf.Max(0.01f, settings.MaxPanUnitsPerSecond) * Time.deltaTime;
        Vector3 next = Vector3.SmoothDamp(cameraTarget.position, _desiredTargetPosition, ref _panVelocity, Mathf.Max(0.001f, settings.PanSmoothSeconds),
            Mathf.Infinity, Time.deltaTime);
        Vector3 delta = next - cameraTarget.position;
        
        if (delta.magnitude > maxDelta)
            next = cameraTarget.position + delta.normalized * maxDelta;

        cameraTarget.position = next;
    }
    
    private void Pan(Vector2 screenDelta)
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0f, cameraTarget.position.y, 0f));

        Ray previewRay = camera.ScreenPointToRay(Input.mousePosition - (Vector3)screenDelta);
        Ray currentRay = camera.ScreenPointToRay(Input.mousePosition);

        if (!plane.Raycast(previewRay, out float prevEnter) || !plane.Raycast(currentRay, out float currEnter))
            return;

        Vector3 worldPreview = previewRay.GetPoint(prevEnter);
        Vector3 worldCurrent = currentRay.GetPoint(currEnter);
        Vector3 deltaWorld = worldPreview - worldCurrent;

        float zoomScale = GetPanZoomScale();
        float effectiveMultiplier = settings.PanMultiplierWorld * zoomScale;

        Vector3 add = deltaWorld * effectiveMultiplier;
        _desiredTargetPosition += new Vector3(add.x, 0f, add.z);
    }
    
    private float GetPanZoomScale()
    {
        if (virtualCamera == null || !settings.ScalePanByZoom)
            return 1f;

        float refSize = Mathf.Max(0.001f, settings.ReferenceOrthoSize);
        float size = virtualCamera.m_Lens.OrthographicSize;
        
        return size / refSize;
    }

    private Vector3 ClampXZ(Vector3 vector, float y)
    {
        vector.x = Mathf.Clamp(vector.x, settings.MinX, settings.MaxX);
        vector.z = Mathf.Clamp(vector.z, settings.MinZ, settings.MaxZ);
        vector.y = y;
        
        return vector;
    }
}