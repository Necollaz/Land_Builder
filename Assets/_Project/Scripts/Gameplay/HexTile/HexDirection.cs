using System.Collections.Generic;
using UnityEngine;

public class HexDirection
{
    private readonly Vector2Int[] _evenColumnOffsets =
    {
        new(1, 0), new(0, -1), new(-1, -1),
        new(-1, 0), new(-1, 1), new(0, 1)
    };
        
    private readonly Vector2Int[] _oddColumnOffsets =
    {
        new(1, 0), new(1, -1), new(0, -1),
        new(-1, 0), new(0, 1), new(1, 1)
    };
        
    public IEnumerable<Vector2Int> GetNeighbors(Vector2Int coordinates)
    {
        Vector2Int[] offsets = (coordinates.y & 1) == 0 ? _evenColumnOffsets : _oddColumnOffsets;

        foreach (Vector2Int offset in offsets)
            yield return coordinates + offset;
    }
}