using UnityEngine;

public class HexSideHelper
{
    private const int WORLD_INDEX_UL = 4;
    
    private readonly Vector2Int[] EvenRowOffsets =
    {
        new(1, 0), new(0, -1), new(-1, -1),
        new(-1, 0), new(-1, 1), new(0, 1)
    };
    private readonly Vector2Int[] OddRowOffsets =
    {
        new(1, 0), new(1, -1), new(0, -1),
        new(-1, 0), new(0, 1), new(1, 1)
    };
    
    public int GetSideIndex(Vector2Int thisCoords, Vector2Int toCoords)
    {
        Vector2Int direction = toCoords - thisCoords;
        bool evenRow = (thisCoords.y & 1) == 0;

        Vector2Int[] offsets = evenRow ? EvenRowOffsets : OddRowOffsets;

        int worldIndex = -1;
        
        for (int i = 0; i < 6; i++)
        {
            if (offsets[i] == direction)
            {
                worldIndex = i;
                
                break;
            }
        }

        if (worldIndex == -1)
        {
            Debug.LogError($"HexSideHelper: неизвестная сторона dir={direction} from={thisCoords} to={toCoords}");
            
            return -1;
        }
        
        int desiredIndex = (worldIndex - WORLD_INDEX_UL + 6) % 6;
        
        return desiredIndex;
    }
}