using UnityEngine;

public static class HexSideHelper
{
    public static int GetSideIndex(Vector2Int thisCoords, Vector2Int toCoords)
    {
        Vector2Int dir = toCoords - thisCoords;
        bool evenRow = (thisCoords.y & 1) == 0;

        Vector2Int[] offsets = evenRow
            ? new[] { new Vector2Int(1,0), new Vector2Int(0,-1), new Vector2Int(-1,-1),
                new Vector2Int(-1,0), new Vector2Int(-1,1), new Vector2Int(0,1) }
            : new[] { new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,-1),
                new Vector2Int(-1,0), new Vector2Int(0,1), new Vector2Int(1,1) };

        int worldIndex = -1;
        for (int i = 0; i < 6; i++)
            if (offsets[i] == dir) { worldIndex = i; break; }

        if (worldIndex == -1)
        {
            Debug.LogError($"HexSideHelper: неизвестная сторона dir={dir} from={thisCoords} to={toCoords}");
            return -1;
        }

        const int WORLD_INDEX_UL = 4;

        int desiredIndex = (worldIndex - WORLD_INDEX_UL + 6) % 6;
        return desiredIndex;
    }
}