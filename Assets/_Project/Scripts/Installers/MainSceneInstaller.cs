using Cinemachine;
using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [Header("Tile Grid Settings")]
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Transform _containerTilePrefabs;
    
    [Header("Scene References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Grid _gridComponent;
    
    [Header("Zoom Settings")]
    [SerializeField] private CinemachineFreeLook _cinemachineFreeLookCamera;
    [SerializeField] private float _zoomSensitivity;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    public override void InstallBindings()
    {
        var cameraController = Bind(new CameraController(_mainCamera));
        var hexDirection = Bind(new HexDirection());
        var gridBuilder = Bind(new HexTileGridBuilder(Container, _gridComponent, _cellPrefab, hexDirection));
        var tilePreview = Bind(new TilePreviewController(_tilePrefab, _containerTilePrefabs));
        var tileRotator = Bind(new TileRotateController());
        var tileSpawn = Bind(new TileSpawnController(tilePreview, tileRotator, gridBuilder, cameraController));
        var hexInitializer = Bind(new HexGridInitializer(gridBuilder, tileSpawn, hexDirection));
        var mouseWheelZoomInput = Bind(new MouseWheelZoomInput(_zoomSensitivity));
        //var TouchPinchZoomInput = Bind(new TouchPinchZoomInput(_zoomSensitivity)); - это под телефоны
        var cinemachineFreeLookCamera = Bind(new CameraZoomController(_cinemachineFreeLookCamera, mouseWheelZoomInput, _zoomSpeed, _minZoom, _maxZoom));
    }

    private T Bind<T>(T controller) where T : class
    {
        Container.BindInterfacesAndSelfTo<T>().FromInstance(controller).AsSingle();
        return controller;
    }
}