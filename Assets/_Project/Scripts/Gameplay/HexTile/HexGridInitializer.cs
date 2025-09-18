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

    private HexGridController _hexGridController;

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
        //tileGridBuilder.Build(Vector2Int.zero);
    }

    void IDisposable.Dispose()
    {
        tileGridBuilder.OnBuildCompleted -= HandleBuildCompleted;
    }

    private void HandleBuildCompleted(Dictionary<Vector2Int, HexCellView> map, Vector2Int position)
    {
        _hexGridController = new HexGridController(map, hexDirection);

        hexagonPutter.SetGridController(_hexGridController);

        tileSpawnController.Initialize(_hexGridController, map);

        tileGridBuilder.OnBuildCompleted -= HandleBuildCompleted;
    }
}