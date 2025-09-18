using UnityEngine;

[CreateAssetMenu(menuName = "Game/Configs/Victory Screen", fileName = "VictoryScreenConfig")]
public class VictoryScreenConfig : ScriptableObject
{
    [Header("Default Reward (fallback if level source not provided)")]
    public int DefaultCoins = 100;

    [Header("World Rotation")]
    public float WorldRotationSpeedDegreesPerSecond = 10f;

    [Header("Flow")]
    public bool GoToMainMenuOnNext = true;
}