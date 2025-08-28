using UnityEngine;

[CreateAssetMenu(fileName = "DeckSettingsConfig", menuName = "Game/Configs/Deck Settings")]
public class DeckSettingsConfig : ScriptableObject
{
    [field: SerializeField, Tooltip("Префаб визуала одной плитки колоды.")] public HexTileView TileViewPrefab { get; private set; }

    [field: Header("Model")]
    [field: SerializeField, Tooltip("Максимально допустимый размер колоды.")] public int DeckMaxSize { get; private set; } = 64;
    [field: SerializeField, Tooltip("Пакет пополнения колоды при опустошении (на сколько элементов пополнять очередь за один раз).")] public int DeckRefillBatch { get; private set; } = 16;
    [field: SerializeField, Tooltip("Максимальное количество различных типов сторон гекса, которые могут встретиться на одной плитке при генерации.")] public int DistinctTypesAllowed { get; private set; } = 2;
    [field: Range(0f, 1f), SerializeField, Tooltip("Вероятность продолжить ту же сторону у следующей грани при генерации (0..1).")] public float ContinueSameChance { get; private set; } = 0.6f;

    [field: Header("View Layout")]
    [field: SerializeField, Tooltip("Базовый локальный сдвиг всей стопки относительно DeckRoot.")] public Vector3 BaseLocalOffset { get; private set; } = Vector3.zero;
    [field: SerializeField, Tooltip("Локальное направление роста стопки (например, (0,1,0) — вверх). Будет нормализовано.")] public Vector3 StackDirectionLocal { get; private set; } = new Vector3(0f, 1f, 0f);
    [field: SerializeField, Tooltip("Базовый шаг между всеми плитками в плотно уложенной части стопки.")] public float PackedStep { get; private set; } = 0.01f;
    [field: SerializeField, Tooltip("Локальный поворот всех плиток в стопке.")] public Vector3 TilesLocalEuler { get; private set; } = Vector3.zero;
    [field: SerializeField, Tooltip("Масштаб для плиток в основной плотно уложенной части стопки.")] public Vector3 HiddenTilesLocalScale { get; private set; } = new Vector3(0.98f, 0.98f, 0.98f);
    [field: SerializeField, Tooltip("Масштаб для трёх верхних плиток стопки.")] public Vector3 TopTilesLocalScale { get; private set; } = Vector3.one;
    
    [field: Header("View Layout - Дополнительные зазоры для верхних трёх")]
    [field: SerializeField, Tooltip("Дополнительный зазор поверх базового шага для самой верхней плитки (наибольшее значение).")] public float Top1ExtraStep { get; private set; } = 0.12f;
    [field: SerializeField, Tooltip("Дополнительный зазор поверх базового шага для второй сверху плитки (меньше, чем у верхней).")] public float Top2ExtraStep { get; private set; } = 0.08f;
    [field: SerializeField, Tooltip("Дополнительный зазор поверх базового шага для третьей сверху плитки (меньше, чем у второй).")] public float Top3ExtraStep { get; private set; } = 0.05f;
    [field: SerializeField, Tooltip("Сколько плиток отображать под верхней тройкой.")] public int VisibleBelowTopCount { get; private set; } = 5;

    [field: Header("Pool")]
    [field: SerializeField, Tooltip("Первичное количество предсозданных экземпляров визуала плитки в пуле.")] public int PoolPreloadCount { get; private set; } = 32;
}