using UnityEngine;

[CreateAssetMenu(menuName = "Game/Configs/Defeat Screen", fileName = "DefeatScreenConfig")]
public class DefeatScreenConfig : ScriptableObject
{
    [Header("Extra Tiles")]
    public int ExtraTilesOnReward = 3;

    [Header("Flow")]
    public bool CloseScreenAfterGrant = true;
}