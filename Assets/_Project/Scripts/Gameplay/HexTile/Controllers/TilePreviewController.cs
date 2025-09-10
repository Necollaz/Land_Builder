using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class TilePreviewController
{
    public event Action OnTilePut;

    private readonly Hexagon tilePrefab;
    private readonly Transform container;
    private readonly TileRotateController rotateController;
   // private readonly CameraController cameraController;

    private Hexagon _previewInstance;
    private Vector2Int? _previewCoordinates;
    private bool _isPreviewActive;

    public Hexagon CurrentPreviewInstance => _previewInstance;
    public Vector2Int? CurrentPreviewCoordinates => _previewCoordinates;
    public bool IsPreviewActive => _isPreviewActive;

    public TilePreviewController(
        Hexagon tilePrefab,
        Transform container,
        TileRotateController rotateController
    )
    {
        this.tilePrefab = tilePrefab;
        this.container = container;
        this.rotateController = rotateController;
       // this.cameraController = cameraController;
    }

    public void StartTilePreview(Vector2Int coordinates, Vector3 worldPosition)
    {
        CancelPreview();

        _previewCoordinates = coordinates;
        _previewInstance = Object.Instantiate(tilePrefab, worldPosition, Quaternion.identity, container);

        _isPreviewActive = true;

        /*if (cameraController != null)
            cameraController.enabled = false; //*/// или cameraController.SetInputEnabled(false);

        OnTilePut?.Invoke();
    }

    public void TickPreviewRotation()
    {
        if (_isPreviewActive && _previewInstance != null)
            rotateController.HandleRotation(_previewInstance);
    }

    public void CancelPreview()
    {
        if (_previewInstance != null)
            Object.Destroy(_previewInstance.gameObject);

        _previewInstance = null;
        _previewCoordinates = null;

        if (_isPreviewActive)
        {
            _isPreviewActive = false;
            /*if (cameraController != null)
                cameraController.enabled = true; // или cameraController.SetInputEnabled(true);*/
        }
    }

    public Hexagon CommitPreview()
    {
        if (_previewInstance == null) return null;

        Hexagon result = _previewInstance;
        _previewInstance = null;
        _previewCoordinates = null;
        _isPreviewActive = false;

        return result;
    }

    
    public void AcceptPreview()
    {
        _previewCoordinates = null;
        _previewInstance = null;

        if (_isPreviewActive)
        {
            _isPreviewActive = false;
            /*if (cameraController != null)
                cameraController.enabled = true; // или cameraController.SetInputEnabled(true);*/
        }
    }
}
