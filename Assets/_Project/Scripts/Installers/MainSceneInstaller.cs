using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [Header("Configs")]
    [SerializeField] private SettingsDefaultsConfig _settingsDefaults;
    [SerializeField] private DeckSettingsConfig _deckSettings;
    [SerializeField] private CameraSettingsConfig _cameraSettings;
    [SerializeField] private SceneNamesData _sceneNamesData;
    
    [Header("Scene References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Grid _gridComponent;
    
    [Header("Tile Grid Settings")]
    [SerializeField] private Hexagon _tilePrefab;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Transform _containerTilePrefabs;
    [SerializeField] private HexagonPutter _hexagonPutter;
    
    [Header("Deck Settings")]
    [SerializeField] private Transform _deckRoot;
    [SerializeField] private Transform _poolParent;
    
    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraTarget;
    
    [Header("UI")]
    [SerializeField] private ScoreViewer _scoreViewer;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private LevelConfig[] _levelConfigs;
    [SerializeField] private LevelLoader _levelLoader;
    
    public override void InstallBindings()
    {
        var deckRandom = new System.Random();
        var deckModel = Bind(new HexDeckModel(_deckSettings.DeckMaxSize, _deckSettings.DeckRefillBatch, _deckSettings.DistinctTypesAllowed,
            _deckSettings.ContinueSameChance, deckRandom));
        var tilePool = Bind(new GameObjectPool<HexTileView>(_deckSettings.TileViewPrefab, _poolParent != null ? _poolParent : _deckRoot,
            _deckSettings.PoolPreloadCount));
        var deckView = Bind(new HexDeckView(_deckRoot, tilePool, _deckSettings.BaseLocalOffset, _deckSettings.StackDirectionLocal,
            _deckSettings.HiddenTilesLocalScale, _deckSettings.TopTilesLocalScale, Quaternion.Euler(_deckSettings.TilesLocalEuler),
            _deckSettings.PackedStep, _deckSettings.Top1ExtraStep, _deckSettings.Top2ExtraStep, _deckSettings.Top3ExtraStep,
            _deckSettings.VisibleBelowTopCount, _deckSettings.DeckShiftSeconds));
        var deckController = Bind(new HexDeckController(deckModel, deckView, _deckSettings.DeckMaxSize));
        var cameraController = Bind(new CameraController(_mainCamera));
        var cameraTouchMover = Bind(new TouchDragPanInput(_cameraSettings.MobilePanSensitivity, _cameraSettings.MobilePanInvert, _cameraSettings.IgnoreWhenOverUI));
        var cameraMouseMover = Bind(new MouseDragPanInput(_cameraSettings.DesktopPanSensitivity, _cameraSettings.DesktopPanInvert, _cameraSettings.IgnoreWhenOverUI));
        var moverAggregator = Bind(new PanInputAggregator(cameraMouseMover, cameraTouchMover));
        var cameraPanWithLag = Bind(new CameraPanWithLag(_cameraTarget, _camera, moverAggregator, _cinemachineVirtualCamera, _cameraSettings));
        var mouseChordedYawInput = Bind(new MouseChordedLRDragYawInput(_cameraSettings, _cameraSettings.IgnoreWhenOverUI));
        var touchTwoFinger = Bind(new TouchTwistAndPinchGestureInput(_cameraSettings.MobileRotateSensitivity, _cameraSettings.MobilePinchZoomSensitivity, _cameraSettings.IgnoreWhenOverUI));
        var rotatorInputAggregator = Bind(new RotateInputAggregator(mouseChordedYawInput, touchTwoFinger));
        var orbitYawAroundTarget = Bind(new OrbitYawAroundTarget(_cameraTarget, rotatorInputAggregator, _cinemachineVirtualCamera, _cameraSettings));
        var mouseDragZoomInput = Bind(new MouseHoldDragZoomInput(_cameraSettings.DesktopWheelZoomSensitivity, _cameraSettings.IgnoreWhenOverUI));
        var zoomAggregator = Bind(new ZoomInputAggregator(mouseDragZoomInput, touchTwoFinger));
        var hexDirection = Bind(new HexDirection());
        var sideHelper = Bind(new HexSideHelper());
        var gridBuilder = Bind(new HexTileGridBuilder(Container, _gridComponent, _cellPrefab, hexDirection));
        var tileRotator = Bind(new TileRotateController());
        var tilePreview = Bind(new TilePreviewController(_tilePrefab, _containerTilePrefabs, tileRotator, deckModel));
        var tileSpawn   = Bind(new TileSpawnController(tilePreview, _gridComponent));
        var score = Bind(new Score());
        var scoreGiver = Bind(new ScoreGiver(score));
        
        Container.Inject(_hexagonPutter);
        
        var hexInitializer = Bind(new HexGridInitializer(gridBuilder, tileSpawn, hexDirection, _hexagonPutter));
        var orthoZoomWithCinemachine = Bind(new OrthoZoomWithCinemachine(_cinemachineVirtualCamera, zoomAggregator, _cameraSettings.ZoomSpeed, _cameraSettings.MinZoom, _cameraSettings.MaxZoom));
        var deckPlacementListener = Bind(new DeckPlacementListener(gridBuilder, deckController));
        
        var progressService = Bind(new ProgressService());

        _levelLoader.Construct(score, gridBuilder, hexInitializer, _loadingScreen);
        var levelSelectionController = Bind(new LevelSelectionController(_levelLoader, progressService));

        BindPlacementBudgetSubsystem(gridBuilder);
    }
    
    private void BindPlacementBudgetSubsystem(HexTileGridBuilder gridBuilder)
    {
        int startPlacements = Mathf.Max(1, _deckSettings.StartPlacements);
        var budget = Bind(new PlacementBudgetModel(startPlacements));

        Container.Bind<PlacementBudgetListener>().AsSingle().WithArguments(gridBuilder, budget);
        Container.Unbind<IExtraTilesGranter>();
        Container.Bind<IExtraTilesGranter>().To<ExtraTilesGranterFromBudget>().AsSingle().WithArguments(budget);
    }
    
    private T Bind<T>(T controller) where T : class
    {
        Container.BindInterfacesAndSelfTo<T>().FromInstance(controller).AsSingle();
        
        return controller;
    }
}