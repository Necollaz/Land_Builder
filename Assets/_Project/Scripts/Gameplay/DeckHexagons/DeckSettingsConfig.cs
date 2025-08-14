using UnityEngine;

[CreateAssetMenu(fileName = "DeckSettingsConfig", menuName = "Game/Configs/Deck Settings")]
public class DeckSettingsConfig : ScriptableObject
{
    [field: SerializeField] public HexTileView TileViewPrefab { get; private set; }

    [field: Header("Model")]
    [field: SerializeField] public int DeckMaxSize { get; private set; } = 64;
    [field: SerializeField] public int DeckRefillBatch { get; private set; } = 16;
    [field: SerializeField] public int DistinctTypesAllowed { get; private set; } = 2;
    [field: Range(0f, 1f)] [field: SerializeField] public float ContinueSameChance { get; private set; } = 0.6f;

    [field: Header("View Layout")]
    [field: SerializeField] public Vector3 BaseLocalOffset { get; private set; } = Vector3.zero;
    [field: SerializeField] public Vector3 StackDirectionLocal { get; private set; } = new Vector3(0f, 1f, 0f);
    [field: SerializeField] public Vector3 HiddenTilesLocalScale { get; private set; } = new Vector3(0.98f, 0.98f, 0.98f);
    [field: SerializeField] public Vector3 TopTilesLocalScale { get; private set; } = Vector3.one;
    [field: SerializeField] public Vector3 TilesLocalEuler { get; private set; } = Vector3.zero;
    [field: SerializeField] public int VisibleTopCount { get; private set; } = 3;
    [field: SerializeField] public float PackedStep { get; private set; } = 0.01f;
    [field: SerializeField] public float VisibleStep { get; private set; } = 0.12f;

    [field: Header("Pool")]
    [field: SerializeField] public int PoolPreloadCount { get; private set; } = 32;
}