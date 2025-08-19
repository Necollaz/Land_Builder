using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [Header("Target Scene")]
    [SerializeField] private string _sceneToLoad = "SceneTestNik";
    [SerializeField] private string _addressablesSceneKey = "SceneTestNik";
    
    [Header("Behavior")]
    [SerializeField] private float _minShowTime = 0.8f;
    [SerializeField] private float _extraHoldTime = 10.0f;
    [SerializeField] private bool _activateImmediatelyWhenReady = false;
    [SerializeField] private bool _allowActivationAfterMinTime = true;

    private IEnumerator Start()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        float startTime = Time.unscaledTime;
        
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(_sceneToLoad);
        sceneAsync.allowSceneActivation = false;
        
        while (sceneAsync.progress < 0.9f)
            yield return null;
        
        if (_activateImmediatelyWhenReady)
        {
            sceneAsync.allowSceneActivation = true;
            
            yield break;
        }
        
        while (Time.unscaledTime - startTime < _minShowTime)
            yield return null;
        //
        if (_extraHoldTime > 0f)
            yield return new WaitForSecondsRealtime(_extraHoldTime);
        //
        if (_allowActivationAfterMinTime)
            sceneAsync.allowSceneActivation = true;
    }
    
    public void ActivateNow()
    {
        StartCoroutine(ActivateCoroutine());
    }

    private IEnumerator ActivateCoroutine()
    {
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(_sceneToLoad);
        
        sceneAsync.allowSceneActivation = true;
        
        yield return null;
    }
}