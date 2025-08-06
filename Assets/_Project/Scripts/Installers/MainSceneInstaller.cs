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

    public override void InstallBindings()
    {
        var cameraController = Bind(new CameraController(_mainCamera));
        var hexDirection = Bind(new HexDirection());
        var gridBuilder = Bind(new HexTileGridBuilder(Container, _gridComponent, _cellPrefab, hexDirection));
        var tilePreview = Bind(new TilePreviewController(_tilePrefab, _containerTilePrefabs));
        var tileRotator = Bind(new TileRotateController());
        var tileSpawn = Bind(new TileSpawnController(tilePreview, tileRotator, gridBuilder, cameraController));
        var hexInitializer = Bind(new HexGridInitializer(gridBuilder, tileSpawn, hexDirection));
    }

    private T Bind<T>(T controller) where T : class
    {
        Container.BindInterfacesAndSelfTo<T>().FromInstance(controller).AsSingle();
        return controller;
    }
}