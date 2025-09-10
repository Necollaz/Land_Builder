using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HexagonPutter : MonoBehaviour
{
    [SerializeField] private RectTransform _hexPutterRect;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _agreeButton;

    private TilePreviewController _tilePreviewController;
    private TileSpawnController _tileSpawnController;
    private HexTileGridBuilder _tileGridBuilder;
    private HexGridController _hexGridController;
    private CameraController _cameraController;
    private Vector2Int _pendingCoordinates;
    private HexCellView _pendingCell;
    private ScoreGiver _scoreGiver;
    private bool _hasPending;
    
    [Inject]
    public void Construct(
        TileSpawnController tileSpawnController,
        TilePreviewController tilePreviewController,
        HexTileGridBuilder tileGridBuilder,
        CameraController cameraController,
        ScoreGiver scoreGiver)
    {
        _tileSpawnController = tileSpawnController;
        _tilePreviewController = tilePreviewController;
        _tileGridBuilder = tileGridBuilder;
        _cameraController = cameraController;
        _scoreGiver = scoreGiver;
    }
    
    private void OnEnable()
    {
        _tileSpawnController.OnTilePut += SetHexagon;
        _tileSpawnController.OnTileCanceled += SetActivePanelFalse;
        
        _agreeButton.onClick.AddListener(OnAgreeClicked);
        _cancelButton.onClick.AddListener(OnCancelClicked);
        
        SetActivePanelFalse();
    }

    private void OnDisable()
    {
        _tileSpawnController.OnTilePut -= SetHexagon;
        _tileSpawnController.OnTileCanceled -= SetActivePanelFalse;
        
        _agreeButton.onClick.RemoveListener(OnAgreeClicked);
        _cancelButton.onClick.RemoveListener(OnCancelClicked);
    }
    
    public void SetGridController(HexGridController hexGridController)
    {
        _hexGridController = hexGridController;
    }

    private void SetHexagon(Vector2Int coordinates, HexCellView cell)
    {
        _pendingCoordinates = coordinates;
        _pendingCell = cell;
        _hasPending = true;
        
        Vector3 cellCenter = _tileGridBuilder.Grid.GetCellCenterWorld(new Vector3Int(coordinates.x, coordinates.y, 0));
        _tilePreviewController.StartTilePreview(coordinates, cellCenter);

        _agreeButton.interactable = true;
        SetActivePanelTrue();
    }
    
    private void CheckNeighbors(Vector2Int coords, Hexagon candidate)
    {
        foreach (var neighborCoords in _tileGridBuilder.GetNeighbors(coords))
        {
            if (!_hexGridController.TryGetTileAt(neighborCoords, out var neighborHex))
                continue;
            
            int sideNew = HexSideHelper.GetSideIndex(coords, neighborCoords);
            int sideExist = HexSideHelper.GetSideIndex(neighborCoords, coords);

            HexType newType  = candidate.GetEdgeType(sideNew);
            HexType existType = neighborHex.GetEdgeType(sideExist);

            _scoreGiver.AddScore(newType, existType);
            
            Debug.Log($"DEBUG NEIGH: {coords}->{neighborCoords} : dirNew={(neighborCoords-coords)} sideNew={sideNew} rotNew={candidate.RotationSteps} " +
                      $"localNew={(sideNew - candidate.RotationSteps +6)%6} typeNew={newType} ; " +
                      $"dirExist={(coords-neighborCoords)} sideExist={sideExist} rotExist={neighborHex.RotationSteps} " +
                      $"localExist={(sideExist - neighborHex.RotationSteps +6)%6} typeExist={existType}" +
                      $" Совпадает {newType == existType} ");

        }
    }
    
    private void OnAgreeClicked()
    {
        if (!_hasPending || _pendingCell == null) return;

        if (_hexGridController == null)
        {
            Debug.LogWarning("HexagonPutter: HexGridController ещё не готов. Подтверждение отклонено.");
            return;
        }

        Hexagon newHex = _tilePreviewController.CommitPreview();

        CheckNeighbors(_pendingCoordinates, newHex);

        _tileGridBuilder.Build(_pendingCoordinates);
        _hexGridController.PlaceTileAt(_pendingCoordinates, newHex);
        _tilePreviewController.AcceptPreview();
        _tileGridBuilder.RemoveCell(_pendingCoordinates);
        _hexGridController.ShowFrontier();

        ClearPending();
        SetActivePanelFalse();
    }


    private void OnCancelClicked()
    {
        _tilePreviewController.CancelPreview();
        if (_pendingCell != null) _pendingCell.SetVisible(true);

        ClearPending();
        SetActivePanelFalse();
    }

    
    private void SetActivePanelTrue() => _hexPutterRect.gameObject.SetActive(true);

    private void SetActivePanelFalse()
    {
        _hexPutterRect.gameObject.SetActive(false);
        _agreeButton.interactable = false;
    }

    private void ClearPending() => _hasPending = false;
}
