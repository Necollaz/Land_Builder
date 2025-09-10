using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [SerializeField] private List<HexagonPart> _parts;
    
    private readonly HexType[] edgeTypes = new HexType[6];
    private readonly float rotationAngel = 60f;
    private readonly int sidesCount = 6;

    public int RotationSteps { get; private set; }

    private void Awake()
    {
        DetermineParts();
    }

    public void RotateSteps(int delta)
    {
        SetRotationSteps(RotationSteps + delta);
    }

    public HexType GetEdgeType(int sideInWorld)
    {
        int localIndex = (sideInWorld - RotationSteps + sidesCount) % sidesCount;
        
        return edgeTypes[localIndex];
    }
    
    private void SetRotationSteps(int steps)
    {
        RotationSteps = (steps % sidesCount + sidesCount) % sidesCount;
        transform.rotation = Quaternion.Euler(0f, RotationSteps * rotationAngel, 0f);
    }

    private void DetermineParts()
    {
        List<HexagonPart> outerParts = _parts
            .Where(p => (int)p.Side >= 0 && (int)p.Side < sidesCount) 
            .ToList();

        if (outerParts.Count != sidesCount)
        {
            outerParts = transform.GetComponentsInChildren<HexagonPart>()
                .Where(p =>
                {
                    Vector3 local = transform.InverseTransformPoint(p.transform.position);
                    
                    return new Vector2(local.x, local.z).magnitude > 0.01f;
                })
                .ToList();
        }

        List<(HexagonPart part, float angle)> list = new List<(HexagonPart part, float angle)>();
        
        foreach (HexagonPart part in outerParts)
        {
            Vector3 local = transform.InverseTransformPoint(part.transform.position);

            float angle = Mathf.Atan2(local.z, local.x);
            
            list.Add((part, angle));
        }
        
        float upperLeftAngle = 3f * Mathf.PI / 4f;
        
        var normalized = list
            .Select(item =>
            {
                float delta = item.angle - upperLeftAngle;
                
                while (delta < 0) 
                    delta += 2 * Mathf.PI;
                
                while (delta >= 2 * Mathf.PI) 
                    delta -= 2 * Mathf.PI;

                float clockwise = (2 * Mathf.PI - delta) % (2 * Mathf.PI);
                
                return (part: item.part, sortKey: clockwise);
            })
            .OrderBy(x => x.sortKey)
            .ToList();

        for (int i = 0; i < sidesCount; i++)
        {
            if (i < normalized.Count)
                edgeTypes[i] = normalized[i].part.Type;
            else
                edgeTypes[i] = HexType.Grass;
        }
        
        int guessed = Mathf.RoundToInt(transform.eulerAngles.y / rotationAngel);
        
        SetRotationSteps(guessed);
    }
}