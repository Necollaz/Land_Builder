using System.Collections.Generic;
using UnityEngine;

public class HexGridController
{
    private readonly Dictionary<Vector2Int, HexCellView> cellViewMap;
    private readonly HexDirection hexDirection;

    private HashSet<Vector2Int> _placedCoordinates = new();
    private HashSet<Vector2Int> _activatedCoordinates = new();

    public HexGridController(Dictionary<Vector2Int, HexCellView> cellViewMap, HexDirection hexDirection)
    {
        this.cellViewMap = cellViewMap;
        this.hexDirection = hexDirection;
    }

    public bool IsCoordinateActive(Vector2Int queryCoordinates) => _activatedCoordinates.Contains(queryCoordinates);

    public void ShowFrontier() => RefreshView();

    public void ShowSingleCellFrontier(Vector2Int centerCoordinates)
    {
        _placedCoordinates.Clear();
        _activatedCoordinates.Clear();
        _activatedCoordinates.Add(centerCoordinates);

        RefreshView();
    }

    public void PlaceTileAt(Vector2Int targetCoordinates)
    {
        if (_placedCoordinates.Contains(targetCoordinates))
            return;

        _placedCoordinates.Add(targetCoordinates);
        _activatedCoordinates.Remove(targetCoordinates);

        foreach (Vector2Int neighbor in hexDirection.GetNeighbors(targetCoordinates))
        {
            if (cellViewMap.ContainsKey(neighbor) && !_placedCoordinates.Contains(neighbor))
                _activatedCoordinates.Add(neighbor);
        }

        RefreshView();
    }

    private void RefreshView()
    {
        foreach (var keyValuePair in cellViewMap)
        {
            bool shouldBeVisible = _activatedCoordinates.Contains(keyValuePair.Key);

            keyValuePair.Value.SetVisible(shouldBeVisible);
        }
    }
}