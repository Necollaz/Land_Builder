using UnityEngine;

[RequireComponent(typeof(Transform))]
public class HexTileView : MonoBehaviour
{
    // Ожидается, что в префабе есть 6 дочерних объектов с MeshRenderer'ами,
    // в порядке Side0..Side5 (индексы совпадают с HexSide enum).
    [Tooltip("Материалы для типов. Должно совпадать по индексу с HexType enum.")]
    public Material[] TypeMaterials;

    private MeshRenderer[] sideRenderers = new MeshRenderer[6];

    void Awake()
    {
        // ищем 6 первых MeshRenderer'ов в дочерних объектах (жёсткое требование для простоты)
        for (int i = 0; i < 6; i++)
        {
            var child = transform.Find($"Side{i}");
            if (child != null)
            {
                sideRenderers[i] = child.GetComponent<MeshRenderer>();
            }
            else
            {
                Debug.LogWarning($"HexTileView: child Side{i} not found on {gameObject.name}");
            }
        }
    }

    public void ApplyData(HexTileData data)
    {
        for (int i = 0; i < 6; i++)
        {
            if (sideRenderers[i] != null)
            {
                int typeIndex = (int)data.SideTypes[i];
                if (typeIndex >= 0 && typeIndex < TypeMaterials.Length)
                {
                    sideRenderers[i].material = TypeMaterials[typeIndex];
                }
            }
        }
    }
}