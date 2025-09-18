using UnityEngine;

[CreateAssetMenu(fileName = "DeckSettingsConfig", menuName = "Game/Configs/Deck Settings")]
public class DeckSettingsConfig : ScriptableObject
{
    [SerializeField] private HexTileView _tileViewPrefab;

    [Header("Model")]
    [SerializeField, Tooltip("Максимально допустимый размер колоды.")] private int _deckMaxSize = 64;
    [SerializeField, Tooltip("Пакет пополнения колоды при опустошении (на сколько элементов пополнять очередь за один раз).")] private int _deckRefillBatch  = 16;
    [SerializeField, Tooltip("Максимальное количество различных типов сторон гекса, которые могут встретиться на одной плитке при генерации.")] private int _distinctTypesAllowed = 2;
    [Range(0f, 1f), SerializeField, Tooltip("Вероятность продолжить ту же сторону у следующей грани при генерации (0..1).")] private float _continueSameChance = 0.6f;

    [Header("Gameplay")]
    [Min(1), SerializeField, Tooltip("Стартовое количество размещений плиток на уровень (бюджет ходов).")] private int _startPlacements = 20;
    
    [Header("View Layout")]
    [SerializeField, Tooltip("Базовый локальный сдвиг всей стопки относительно DeckRoot.")] private Vector3 _baseLocalOffset = Vector3.zero;
    [SerializeField, Tooltip("Локальное направление роста стопки (например, (0,1,0) — вверх). Будет нормализовано.")] private Vector3 _stackDirectionLocal = new Vector3(0f, 1f, 0f);
    [SerializeField, Tooltip("Базовый шаг между всеми плитками в плотно уложенной части стопки.")] private float _packedStep = 0.15f;
    [SerializeField, Tooltip("Локальный поворот всех плиток в стопке.")] private Vector3 _tilesLocalEuler = Vector3.zero;
    [SerializeField, Tooltip("Масштаб для плиток в основной плотно уложенной части стопки.")] private Vector3 _hiddenTilesLocalScale = new Vector3(1f, 1f, 1f);
    [SerializeField, Tooltip("Масштаб для трёх верхних плиток стопки.")] private Vector3 _topTilesLocalScale = Vector3.one;
    
    [Header("View Layout - Дополнительные зазоры для верхних трёх")]
    [SerializeField, Tooltip("Дополнительный зазор поверх базового шага для самой верхней плитки (наибольшее значение).")] private float _top1ExtraStep = 0.55f;
    [SerializeField, Tooltip("Дополнительный зазор поверх базового шага для второй сверху плитки (меньше, чем у верхней).")] private float _top2ExtraStep = 0.35f;
    [SerializeField, Tooltip("Дополнительный зазор поверх базового шага для третьей сверху плитки (меньше, чем у второй).")] private float _top3ExtraStep = 0.15f;
    [SerializeField, Tooltip("Сколько плиток отображать под верхней тройкой.")] private int _visibleBelowTopCount = 3;
    
    [Header("View Animation")]
    [SerializeField, Tooltip("Время плавного сдвига стопки после выкладки верхней карты, сек.")] private float _deckShiftSeconds = 0.15f;

    [Header("Pool")]
    [SerializeField] private int _poolPreloadCount = 32;

    public HexTileView TileViewPrefab => _tileViewPrefab;
    public int DeckMaxSize => _deckMaxSize;
    public int DeckRefillBatch => _deckRefillBatch;
    public int DistinctTypesAllowed  => _distinctTypesAllowed;
    public float ContinueSameChance => _continueSameChance;
    public int StartPlacements => _startPlacements;
    public Vector3 BaseLocalOffset => _baseLocalOffset;
    public Vector3 StackDirectionLocal  => _stackDirectionLocal;
    public float PackedStep => _packedStep;
    public Vector3 TilesLocalEuler  => _tilesLocalEuler;
    public Vector3 HiddenTilesLocalScale => _hiddenTilesLocalScale;
    public Vector3 TopTilesLocalScale => _topTilesLocalScale;
    public float Top1ExtraStep => _top1ExtraStep;
    public float Top2ExtraStep => _top2ExtraStep;
    public float Top3ExtraStep => _top3ExtraStep;
    public int VisibleBelowTopCount  => _visibleBelowTopCount;
    public float DeckShiftSeconds => _deckShiftSeconds;
    public int PoolPreloadCount => _poolPreloadCount;
}