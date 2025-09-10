using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HexTileGridBuilder
{
    public delegate void OnTileBuild(Dictionary<Vector2Int, HexCellView> map, Vector2Int position);
    public event OnTileBuild OnBuildCompleted;
    
    private readonly Grid grid;
    private readonly GameObject cellPrefab;
    private readonly DiContainer container;
    private readonly HexDirection hexDirection;
    
    private Dictionary<Vector2Int, HexCellView> _mapCellViews = new();
    private HashSet<Vector2Int> _placed = new();
    private HashSet<Vector2Int> _activated = new();
    
    private bool _initialised = false;
    
    public HexDirection Direction => hexDirection;
    
    [Inject]
    public HexTileGridBuilder(DiContainer container, Grid grid, GameObject cellPrefab, HexDirection hexDirection)
    {
        this.container = container;
        this.grid = grid;
        this.cellPrefab = cellPrefab;
        this.hexDirection = hexDirection;
    }
    
    public Grid Grid => grid;

    public IEnumerable<Vector2Int> GetNeighbors(Vector2Int coordinates)
    {
        return hexDirection.GetNeighbors(coordinates);
    }
    
    public void Build(Vector2Int coordinates)
    {
        if (!_initialised)
        {
            InstantiateCell(Vector2Int.zero);
            _activated.Add(Vector2Int.zero);
            
            _initialised = true;
            
            RefreshView();
            
            OnBuildCompleted?.Invoke(_mapCellViews, Vector2Int.zero);
            
            return;
        }
        
        if (_placed.Contains(coordinates))
            return;
        
        _placed.Add(coordinates);
        _activated.Remove(coordinates);
        
        foreach (Vector2Int neighbor in hexDirection.GetNeighbors(coordinates))
        {
            if (!_placed.Contains(neighbor))
            {
                _activated.Add(neighbor);
                
                if (!_mapCellViews.ContainsKey(neighbor))
                    InstantiateCell(neighbor);
            }
        }
        
        RefreshView();
        
        OnBuildCompleted?.Invoke(_mapCellViews, coordinates);
    }
    
    public void RemoveCell(Vector2Int coordinates)
    {
        if (_mapCellViews.TryGetValue(coordinates, out HexCellView view))
        {
            Object.Destroy(view.gameObject);
            _mapCellViews.Remove(coordinates);
        }
    }
    
    private void InstantiateCell(Vector2Int coordinates)
    {
        Vector3Int cellPosition = new Vector3Int(coordinates.x, coordinates.y, 0);
        Vector3 worldPoint = grid.GetCellCenterWorld(cellPosition);

        GameObject cellObject = container.InstantiatePrefab(cellPrefab, worldPoint, Quaternion.identity, grid.transform);

        if (cellObject.TryGetComponent(out HexCellView view))
            _mapCellViews[coordinates] = view;
    }

    private void RefreshView()
    {
        foreach (var kv in _mapCellViews)
            kv.Value.SetVisible(_activated.Contains(kv.Key));
    }
}