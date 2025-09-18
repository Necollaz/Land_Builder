using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public int LevelId { get; private set; }
    
    [field: SerializeField] public int RewardCoins { get; private set; }
    
    [field: SerializeField] public int RequiredScore { get; private set; }
    
    [field: SerializeField] public int StartHexagons { get; private set; }
    
    [field: SerializeField] public List<LevelConfig> NextLevels { get; private set; }
}
