using UnityEngine;

[CreateAssetMenu(menuName = "Game/Configs/Scene Names Data", fileName = "SceneNamesData")]
public class SceneNamesData : ScriptableObject
{
    public string MainMenuSceneName = "MainMenuScene";
    public string GameSceneName = "GameScene";
    public string LoadingSceneName = "SplashScene";
}