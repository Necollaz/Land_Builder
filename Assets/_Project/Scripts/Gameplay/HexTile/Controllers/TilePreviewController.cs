using UnityEngine;
using Object = UnityEngine.Object;

public class TilePreviewController
{
    private readonly GameObject tilePrefab;
    private readonly Transform container;
    
    private GameObject _previewInstance;
    private Vector2Int? _previewCoordinates;

    public GameObject CurrentPreviewInstance => _previewInstance;
    public Vector2Int? CurrentPreviewCoordinates => _previewCoordinates;
    
    public TilePreviewController(GameObject tilePrefab, Transform container)
    {
        this.tilePrefab = tilePrefab;
        this.container = container;
    }

    public void StartTilePreview(Vector2Int coordinates, Vector3 worldPosition)
    {
        CancelPreview();

        _previewCoordinates = coordinates;
        _previewInstance = Object.Instantiate(tilePrefab, worldPosition, Quaternion.identity, container);
    }

    public void CancelPreview()
    {
        if (_previewInstance != null)
            Object.Destroy(_previewInstance.gameObject);

        _previewInstance = null;
        _previewCoordinates = null;
    }

    public void AcceptPreview()
    {
        _previewCoordinates = null;
        _previewInstance = null;
    }
}