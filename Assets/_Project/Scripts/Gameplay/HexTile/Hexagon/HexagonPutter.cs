using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HexagonPutter : MonoBehaviour
{
    [SerializeField] private RectTransform _hexPutterRect;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _agreeButton;
    
    [Header("Mobile Rotate UI")]
    [SerializeField] private Button _rotateLeftButton;
    [SerializeField] private Button _rotateRightButton;
    [SerializeField] private GameObject _rotateButtonsContainer;

    private TilePreviewController _tilePreviewController;
    private TileSpawnController _tileSpawnController;
    private HexTileGridBuilder _tileGridBuilder;
    private HexGridController _hexGridController;
    private HexSideHelper _sideHelper;
    private CameraController _cameraController;
    private HexCellView _pendingCell;
    private ScoreGiver _scoreGiver;
    
    private Vector2Int _pendingCoordinates;
    
    private bool _hasPending;
    private bool _subscribed;
    
    [Inject]
    public void Construct(TileSpawnController tileSpawnController, TilePreviewController tilePreviewController,
        HexTileGridBuilder tileGridBuilder, CameraController cameraController, ScoreGiver scoreGiver, HexSideHelper sideHelper)
    {
        _tileSpawnController = tileSpawnController;
        _tilePreviewController = tilePreviewController;
        _tileGridBuilder = tileGridBuilder;
        _cameraController = cameraController;
        _scoreGiver = scoreGiver;
        _sideHelper = sideHelper;
    }
    
    private void OnEnable()
    {
        if (_tileSpawnController != null)
        {
            _tileSpawnController.OnTilePut += SetHexagon;
            _tileSpawnController.OnTileCanceled += SetActivePanelFalse;
            _tileSpawnController.OnTileAcceptRequested += OnAgreeClicked;
            _tileSpawnController.OnTileCancelRequested += OnCancelClicked;
            _subscribed = true;
        }
        else
        {
            Debug.LogWarning($"{nameof(HexagonPutter)}: TileSpawnController is not injected yet.");
        }

        if (_agreeButton != null)
            _agreeButton.onClick.AddListener(OnAgreeClicked);
        
        if (_cancelButton != null)
            _cancelButton.onClick.AddListener(OnCancelClicked);

        if (_rotateLeftButton != null)
            _rotateLeftButton.onClick.AddListener(() => _tilePreviewController?.RotatePreviewStep(-1));
        if (_rotateRightButton != null)
            _rotateRightButton.onClick.AddListener(() => _tilePreviewController?.RotatePreviewStep(+1));

        SetActivePanelFalse();
        SetRotateButtonsVisible(false);
    }

    private void OnDisable()
    {
        if (_subscribed && _tileSpawnController != null)
        {
            _tileSpawnController.OnTilePut -= SetHexagon;
            _tileSpawnController.OnTileCanceled -= SetActivePanelFalse;
            _tileSpawnController.OnTileAcceptRequested -= OnAgreeClicked;
            _tileSpawnController.OnTileCancelRequested -= OnCancelClicked;
            _subscribed = false;
        }

        if (_agreeButton != null)
            _agreeButton.onClick.RemoveListener(OnAgreeClicked);
        
        if (_cancelButton != null)
            _cancelButton.onClick.RemoveListener(OnCancelClicked);

        if (_rotateLeftButton != null)
            _rotateLeftButton.onClick.RemoveAllListeners();
        
        if (_rotateRightButton != null)
            _rotateRightButton.onClick.RemoveAllListeners();
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
        
#if UNITY_ANDROID || UNITY_IOS
        SetRotateButtonsVisible(true);
#else
        SetRotateButtonsVisible(false);
#endif
    }
    
    private void CheckNeighbors(Vector2Int coords, Hexagon candidate)
    {
        foreach (Vector2Int neighborCoords in _tileGridBuilder.GetNeighbors(coords))
        {
            if (!_hexGridController.TryGetTileAt(neighborCoords, out var neighborHex))
                continue;
            
            int sideNew = _sideHelper.GetSideIndex(coords, neighborCoords);
            int sideExist = _sideHelper.GetSideIndex(neighborCoords, coords);

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
        if (!_hasPending || _pendingCell == null)
            return;

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
        SetRotateButtonsVisible(false);
    }

    private void OnCancelClicked()
    {
        _tilePreviewController.CancelPreview();
        
        if (_pendingCell != null)
            _pendingCell.SetVisible(true);

        ClearPending();
        SetActivePanelFalse();
        SetRotateButtonsVisible(false);
    }

    
    private void SetActivePanelTrue() => _hexPutterRect.gameObject.SetActive(true);
    
    private void ClearPending() => _hasPending = false;
    
    private void SetActivePanelFalse()
    {
        _hexPutterRect.gameObject.SetActive(false);
        _agreeButton.interactable = false;
        
        SetRotateButtonsVisible(false);
    }
    
    private void SetRotateButtonsVisible(bool isVisible)
    {
        if (_rotateButtonsContainer != null)
            _rotateButtonsContainer.SetActive(isVisible);
    }
}