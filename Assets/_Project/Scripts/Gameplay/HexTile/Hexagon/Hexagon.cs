using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    private const float ROTATION_ANGEL = 60f;
    private const int SIDES_COUNT = 6;
    
    [SerializeField] private List<HexagonPart> _parts;
    
    private readonly HexType[] edgeTypes = new HexType[6];

    public int RotationSteps { get; private set; }

    private void Awake()
    {
        CachePartsIntoEdges();
        GuessInitialRotationFromTransform();
    }

    public void RotateSteps(int delta)
    {
        SetRotationSteps(RotationSteps + delta);
    }

    public HexType GetEdgeType(int sideInWorld)
    {
        int localIndex = (sideInWorld - RotationSteps + SIDES_COUNT) % SIDES_COUNT;
        
        return edgeTypes[localIndex];
    }
    
    public void SetSideTypes(HexType[] sideTypesClockwiseFromSide0)
    {
        if (sideTypesClockwiseFromSide0 == null || sideTypesClockwiseFromSide0.Length != SIDES_COUNT)
            throw new System.ArgumentException("Hexagon.SetSideTypes: массив должен содержать ровно 6 элементов.");

        for (int i = 0; i < SIDES_COUNT; i++)
            edgeTypes[i] = sideTypesClockwiseFromSide0[i];
        
        if (_parts != null)
        {
            foreach (HexagonPart part in _parts)
            {
                int sideIndex = (int)part.Side;
                
                if (sideIndex >= 0 && sideIndex < SIDES_COUNT)
                    part.ApplyType(edgeTypes[sideIndex]);
            }
        }
    }
    
    private void SetRotationSteps(int steps)
    {
        RotationSteps = (steps % SIDES_COUNT + SIDES_COUNT) % SIDES_COUNT;
        transform.rotation = Quaternion.Euler(0f, RotationSteps * ROTATION_ANGEL, 0f);
    }

    private void CachePartsIntoEdges()
    {
        if (_parts == null || _parts.Count == 0)
            _parts = new List<HexagonPart>(GetComponentsInChildren<HexagonPart>(true));
        
        for (int i = 0; i < SIDES_COUNT; i++)
            edgeTypes[i] = HexType.Grass;

        foreach (HexagonPart part in _parts)
        {
            int sideIndex = (int)part.Side;
            
            if (sideIndex >= 0 && sideIndex < SIDES_COUNT)
                edgeTypes[sideIndex] = part.Type;
        }
    }

    private void GuessInitialRotationFromTransform()
    {
        int guessed = Mathf.RoundToInt(transform.eulerAngles.y / ROTATION_ANGEL);
        
        SetRotationSteps(guessed);
    }
}