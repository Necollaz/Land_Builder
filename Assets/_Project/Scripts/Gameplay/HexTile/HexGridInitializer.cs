using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HexGridInitializer : IInitializable, IDisposable
{
    private readonly HexTileGridBuilder tileGridBuilder;
    private readonly TileSpawnController tileSpawnController;
    private readonly HexDirection hexDirection;
    private readonly HexagonPutter hexagonPutter;

    public HexGridController HexGridController { get; private set; }

    public HexGridInitializer(
        HexTileGridBuilder tileGridBuilder,
        TileSpawnController tileSpawnController,
        HexDirection hexDirection,
        HexagonPutter hexagonPutter)
    {
        this.tileGridBuilder = tileGridBuilder;
        this.tileSpawnController = tileSpawnController;
        this.hexDirection = hexDirection;
        this.hexagonPutter = hexagonPutter;
    }

    void IInitializable.Initialize()
    {
        tileGridBuilder.OnBuildCompleted += HandleBuildCompleted;
        tileGridBuilder.Build(Vector2Int.zero);
    }

    void IDisposable.Dispose()
    {
        tileGridBuilder.OnBuildCompleted -= HandleBuildCompleted;
    }

    private void HandleBuildCompleted(Dictionary<Vector2Int, HexCellView> map, Vector2Int position)
    {
        HexGridController = new HexGridController(map, hexDirection);

        hexagonPutter.SetGridController(HexGridController);

        tileSpawnController.Initialize(HexGridController, map);

        tileGridBuilder.OnBuildCompleted -= HandleBuildCompleted;
    }
}