using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HexGridInitializer : IInitializable, IDisposable
{
    private readonly HexTileGridBuilder tileGridBuilder;
    private readonly TileSpawnController tileSpawnController;
    private readonly HexDirection hexDirection;
        
    public HexGridInitializer(HexTileGridBuilder tileGridBuilder, TileSpawnController tileSpawnController, HexDirection hexDirection)
    {
        this.tileGridBuilder = tileGridBuilder;
        this.tileSpawnController = tileSpawnController;
        this.hexDirection = hexDirection;
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
        HexGridController hexGridController = new HexGridController(map, hexDirection);
            
        tileSpawnController.Initialize(hexGridController, map);
        
        tileGridBuilder.OnBuildCompleted -= HandleBuildCompleted;
    }
}