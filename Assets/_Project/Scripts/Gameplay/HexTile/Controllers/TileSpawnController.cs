using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileSpawnController : ITickable
{
    private readonly TilePreviewController tilePreviewController;
    private readonly TileRotateController tileRotateController;
    private readonly HexTileGridBuilder tileGridBuilder;
    private readonly CameraController cameraController;
    private readonly HexDirection hexDirection = new ();
    
    private HexGridController _hexGridController;
    private Dictionary<Vector2Int, HexCellView> _cellViewMap;
    private bool _isReady;
    
    public TileSpawnController(TilePreviewController tilePreviewController, TileRotateController tileRotateController, HexTileGridBuilder gridBuilder, CameraController cameraController)
    {
        this.tilePreviewController = tilePreviewController;
        this.tileRotateController = tileRotateController;
        this.tileGridBuilder = gridBuilder;
        this.cameraController = cameraController;
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

        tileRotateController.HandleRotation(tilePreviewController.CurrentPreviewInstance);

        if (!Input.GetMouseButtonDown(0))
            return;

        Camera mainCamera = CameraController.MainCamera;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, 0f);

        if (!groundPlane.Raycast(ray, out float enterDistance))
        {
            CancelOrResetPreview();

            return;
        }

        Vector3 worldPoint = ray.GetPoint(enterDistance);
        
        if (!TryGetClosestCell(worldPoint, out Vector2Int pickedCoordinates) || !_hexGridController.IsCoordinateActive(pickedCoordinates))
        {
            CancelOrResetPreview();
            return;
        }

        if (tilePreviewController.CurrentPreviewInstance != null)
        {
            if (tilePreviewController.CurrentPreviewCoordinates == pickedCoordinates)
            {
                var currentTile = tilePreviewController.CurrentPreviewInstance;

                int dirIndex = 0;
                
                foreach (var neighborCoords in hexDirection.GetNeighbors(pickedCoordinates))
                {
                    if (!_hexGridController.TryGetTileAt(neighborCoords, out Hexagon neighborTile))
                    {
                        dirIndex++;
                        continue;
                    }

                    int oppositeDir = (dirIndex + 3) % 6;

                    var currentType = currentTile.GetEdgeType(dirIndex);
                    var neighborType = neighborTile.GetEdgeType(oppositeDir);

                    if (currentType == neighborType)
                        Debug.Log($"Грань {dirIndex} совпала с соседом {neighborCoords}: {currentType}");
                    else
                        Debug.Log($"Грань {dirIndex} ({currentType}) не совпала с гранью соседа ({neighborType})");
            
                    dirIndex++;
                }
                
                tileGridBuilder.Build(pickedCoordinates);
                _hexGridController.PlaceTileAt(pickedCoordinates, currentTile);
                tilePreviewController.AcceptPreview();
                tileGridBuilder.RemoveCell(pickedCoordinates);
                _hexGridController.ShowFrontier();
            }
            else
            {
                CancelOrResetPreview();
            }
        }
        else
        {
            HexCellView cell = _cellViewMap[pickedCoordinates];
            
            cell.SetVisible(false);
            tilePreviewController.StartTilePreview(pickedCoordinates, cell.transform.position);
        }
    }

    private void CancelOrResetPreview()
    {
        tilePreviewController.CancelPreview();
        _hexGridController.ShowFrontier();
    }

    private bool TryGetClosestCell(Vector3 worldPosition, out Vector2Int closest)
    {
        float minSquaredDistance = float.MaxValue;
        Vector2Int bestMatch = default;
        bool found = false;

        foreach (var keyValuePair in _cellViewMap)
        {
            float squaredDistance = (worldPosition - keyValuePair.Value.transform.position).sqrMagnitude;

            if (squaredDistance < minSquaredDistance)
            {
                minSquaredDistance = squaredDistance;
                bestMatch = keyValuePair.Key;
                found = true;
            }
        }

        closest = bestMatch;
        
        return found;
    }
}