using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TileSpawnController : ITickable
{
    private const float MAX_CLICK_TIME_SECONDS = 0.25f;
    private const float MAX_CLICK_MOVE_SQUARED = 8f * 8f;
    
    public event Action<Vector2Int, HexCellView> OnTilePut;
    public event Action OnTileCanceled;
    public event Action OnTileAcceptRequested;
    public event Action OnTileCancelRequested;

    private readonly TilePreviewController tilePreviewController;
    private readonly Grid grid;
    
    private HexGridController _hexGridController;
    private Dictionary<Vector2Int, HexCellView> _cellViewMap;
    
    private Vector2 _mouseDownPosition;
    private float _mouseDownTime;
    private bool _pendingClick;
    private bool _isReady;

    public TileSpawnController(TilePreviewController tilePreviewController, Grid grid)
    {
        this.tilePreviewController = tilePreviewController;
        this.grid = grid;
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

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            _pendingClick = true;
            _mouseDownTime = Time.unscaledTime;
            _mouseDownPosition = Input.mousePosition;
        }
        
        if (_pendingClick)
        {
            Vector2 delta = (Vector2)Input.mousePosition - _mouseDownPosition;
            
            if (delta.sqrMagnitude > MAX_CLICK_MOVE_SQUARED)
                _pendingClick = false;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            bool validClick = _pendingClick && (Time.unscaledTime - _mouseDownTime) <= MAX_CLICK_TIME_SECONDS;
            _pendingClick = false;

            if (!validClick)
                return;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            Ray ray = CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
            Plane gridPlane = new Plane(grid.transform.up, grid.transform.position);

            if (!gridPlane.Raycast(ray, out float enter))
            {
                ClickWhenNoCellHit();
                
                return;
            }

            Vector3 worldPoint = ray.GetPoint(enter);
            Vector3Int cell = grid.WorldToCell(worldPoint);
            Vector2Int coordinates = new Vector2Int(cell.x, cell.y);
            
            if (tilePreviewController.IsPreviewActive)
            {
                Vector2Int? previewCoords = tilePreviewController.CurrentPreviewCoordinates;
                
                if (!previewCoords.HasValue)
                {
                    ClickWhenNoCellHit();
                    
                    return;
                }

                if (coordinates == previewCoords.Value)
                    OnTileAcceptRequested?.Invoke();
                else
                    OnTileCancelRequested?.Invoke();

                return;
            }
            
            if (!_cellViewMap.TryGetValue(coordinates, out HexCellView cellView) || !_hexGridController.IsCoordinateActive(coordinates))
            {
                CancelOrResetPreview();
                
                return;
            }

            cellView.SetVisible(false);
            OnTilePut?.Invoke(coordinates, cellView);
        }
    }
    
    private void ClickWhenNoCellHit()
    {
        if (tilePreviewController.IsPreviewActive)
        {
            OnTileCancelRequested?.Invoke();
            
            return;
        }

        CancelOrResetPreview();
    }

    private void CancelOrResetPreview()
    {
        tilePreviewController.CancelPreview();
        _hexGridController.ShowFrontier();
        
        OnTileCanceled?.Invoke();
    }
}