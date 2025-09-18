using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator
{
    private readonly SceneNamesData sceneNamesData;

    public SceneNavigator(SceneNamesData sceneNamesData)
    {
        this.sceneNamesData = sceneNamesData ?? throw new System.ArgumentNullException(nameof(sceneNamesData));
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(sceneNamesData.MainMenuSceneName);
    }
    
    public void LoadGameplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNamesData.GameSceneName);
    }

    public void LoadLoadingScene()
    {
        SceneManager.LoadScene(sceneNamesData.LoadingSceneName);
    }
    
    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        
        Scene active = SceneManager.GetActiveScene();
        
        if (active.IsValid())
            SceneManager.LoadScene(active.buildIndex);
    }
}