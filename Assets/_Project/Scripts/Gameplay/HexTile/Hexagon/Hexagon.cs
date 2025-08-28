using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [SerializeField] private List<HexagonPart> _parts;
    
    private readonly HexType[] _edgeTypes = new HexType[6];

    public int RotationSteps { get; private set; }

    private void Awake()
    {
        var outerParts = _parts
            .Where(p => (int)p.Side >= 0 && (int)p.Side < 6) 
            .ToList();

        if (outerParts.Count != 6)
        {
            outerParts = transform.GetComponentsInChildren<HexagonPart>()
                .Where(p =>
                {
                    var local = transform.InverseTransformPoint(p.transform.position);
                    return new Vector2(local.x, local.z).magnitude > 0.01f;
                })
                .ToList();
        }

        if (outerParts.Count != 6)
        {
            Debug.LogWarning($"Hexagon: найдено {outerParts.Count} внешних частей у {name}. Ожидалось 6. Проверь структуру объекта.");
        }

        var list = new List<(HexagonPart part, float angle)>();
        foreach (var p in outerParts)
        {
            Vector3 local = transform.InverseTransformPoint(p.transform.position);

            float angle = Mathf.Atan2(local.z, local.x);
            list.Add((p, angle));
        }
        
        float upperLeftAngle = 3f * Mathf.PI / 4f;
        
        var normalized = list
            .Select(item =>
            {
                float delta = item.angle - upperLeftAngle;
                while (delta < 0) delta += 2 * Mathf.PI;
                while (delta >= 2 * Mathf.PI) delta -= 2 * Mathf.PI;

                float clockwise = (2 * Mathf.PI - delta) % (2 * Mathf.PI);
                return (part: item.part, sortKey: clockwise);
            })
            .OrderBy(x => x.sortKey)
            .ToList();

        for (int i = 0; i < 6; i++)
        {
            if (i < normalized.Count)
            {
                _edgeTypes[i] = normalized[i].part.Type;
            }
            else
            {
                _edgeTypes[i] = HexType.Grass;
            }
        }
        
        int guessed = Mathf.RoundToInt(transform.eulerAngles.y / 60f);
        SetRotationSteps(guessed);
    }

    public void SetRotationSteps(int steps)
    {
        RotationSteps = ((steps % 6) + 6) % 6;
        transform.rotation = Quaternion.Euler(0f, RotationSteps * 60f, 0f);
    }

    public void RotateSteps(int delta)
    {
        SetRotationSteps(RotationSteps + delta);
    }

    public HexType GetEdgeType(int sideInWorld)
    {
        int localIndex = (sideInWorld - RotationSteps + 6) % 6;
        return _edgeTypes[localIndex];
    }
}
