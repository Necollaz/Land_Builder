using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [SerializeField] private List<HexagonPart> _parts;

    public HexType GetEdgeType(int sideIndex)
    {
        int rotationSteps = Mathf.RoundToInt(transform.eulerAngles.y / 60f) % 6;

        int adjustedIndex = (sideIndex - rotationSteps + 6) % 6;

        HexagonPart part = _parts.Find(p => (int)p.Side == adjustedIndex);
        return part.Type;
    }
}