using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [Header("Tile Grid Settings")]
    [SerializeField] private Hexagon _tilePrefab;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Transform _containerTilePrefabs;
    
    [Header("Scene References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Grid _gridComponent;
    
    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraTarget;
    
    [Header("Camera Settings for zoom")]
    [SerializeField] private float _zoomSensitivity;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;
    
    [Header("Camera Settings for rotation")]
    [SerializeField] private float _rotarorSensitivity;
    
    [Header("Camera Settings for movement")]
    [SerializeField] private float _deadZone;
    [SerializeField] private float _panMultiplier;
    [SerializeField] private float _movementSensitivity;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxZ;

    [SerializeField] private HexagonPutter _hexagonPutter;
    
    public override void InstallBindings()
    {
        var cameraController = Bind(new CameraController(_mainCamera));
        var hexDirection = Bind(new HexDirection());
        var gridBuilder = Bind(new HexTileGridBuilder(Container, _gridComponent, _cellPrefab, hexDirection));
        var tileRotator = Bind(new TileRotateController());
        var tilePreview = Bind(new TilePreviewController(_tilePrefab, _containerTilePrefabs, tileRotator));
        var tileSpawn = Bind(new TileSpawnController(tilePreview));
        var hexInitializer = Bind(new HexGridInitializer(gridBuilder, tileSpawn, hexDirection, _hexagonPutter));
        var mouseWheelRotarorInput = Bind(new MouseWheelRotarorInput(_rotarorSensitivity));
        var touchDragRotatorInput = Bind(new TouchDragRotatorInput(_rotarorSensitivity));
        var rotatorInputAggregator = Bind(new RotatorInputAggregator(mouseWheelRotarorInput, touchDragRotatorInput));
        var mouseWheelZoomInput = Bind(new MouseWheelZoomInput(_zoomSensitivity));
        var touchPinchZoomInput = Bind(new TouchPinchZoomInput(_zoomSensitivity));
        var zoomAggregator = Bind(new ZoomAggregator(mouseWheelZoomInput, touchPinchZoomInput));
        var cameraTouchMover = Bind(new CameraTouchMover(_deadZone));
        var cameraMouseMover = Bind(new CameraMouseMover(_movementSensitivity));
        var moverAggregator = Bind(new CameraMoverInputAggregator(cameraMouseMover, cameraTouchMover));

        Bind(new CameraMoverController(_cameraTarget, _camera, moverAggregator, _panMultiplier, _minX, _maxX, _minZ, _maxZ));
        Bind(new CameraZoomController(_cinemachineVirtualCamera, zoomAggregator, _zoomSpeed, _minZoom, _maxZoom));
        Bind(new CameraRotatorController(_cameraTarget, rotatorInputAggregator));

        _hexagonPutter.Init(tileSpawn, tilePreview, gridBuilder, cameraController);
    }

    private T Bind<T>(T controller) where T : class
    {
        Container.BindInterfacesAndSelfTo<T>().FromInstance(controller).AsSingle();
        return controller;
    }
}