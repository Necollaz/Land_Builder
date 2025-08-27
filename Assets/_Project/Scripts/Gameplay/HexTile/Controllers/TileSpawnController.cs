using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileSpawnController : ITickable
{
    public event Action<Vector2Int, HexCellView> OnTilePut;
    public event Action OnTileCanceled;

    private readonly TilePreviewController tilePreviewController;
    private HexGridController _hexGridController;
    private Dictionary<Vector2Int, HexCellView> _cellViewMap;
    private bool _isReady;

    public TileSpawnController(TilePreviewController tilePreviewController)
    {
        this.tilePreviewController = tilePreviewController;
    }

    public void Initialize(HexGridController hexGridController, Dictionary<Vector2Int, HexCellView> cellViewMap)
    {
        _hexGridController = hexGridController;
        _cellViewMap = cellViewMap;
        _isReady = true;
        _hexGridController.ShowSingleCellFrontier(new Vector2Int(0, 0));
    }

    void ITickable.Tick()
    {
        if (!_isReady)
            return;

        tilePreviewController.TickPreviewRotation();

        if (tilePreviewController.IsPreviewActive)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;
        
        if (!_isReady || !Input.GetMouseButtonDown(0))
            return;

        if (tilePreviewController.CurrentPreviewInstance != null)
            return;

        var ray = CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, 0f);

        if (!plane.Raycast(ray, out var enter))
        {
            CancelOrResetPreview();
            return;
        }

        var worldPoint = ray.GetPoint(enter);

        if (!TryGetClosestCell(worldPoint, out var picked) || !_hexGridController.IsCoordinateActive(picked))
        {
            CancelOrResetPreview();
            return;
        }

        var cell = _cellViewMap[picked];
        cell.SetVisible(false);
        OnTilePut?.Invoke(picked, cell);
    }

    private void CancelOrResetPreview()
    {
        tilePreviewController.CancelPreview();
        _hexGridController.ShowFrontier();
        OnTileCanceled?.Invoke();
    }

    private bool TryGetClosestCell(Vector3 worldPosition, out Vector2Int closest)
    {
        float min = float.MaxValue;
        Vector2Int best = default;
        bool found = false;

        foreach (var kv in _cellViewMap)
        {
            var dist = (worldPosition - kv.Value.transform.position).sqrMagnitude;
            if (dist < min)
            {
                min = dist;
                best = kv.Key;
                found = true;
            }
        }
        closest = best;
        return found;
    }
}
