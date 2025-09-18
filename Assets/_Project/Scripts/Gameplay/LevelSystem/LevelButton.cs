using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private LevelConfig _config;
    
    [SerializeField] private LevelLoader _loader;
    
    public void Construct(LevelLoader loader)
    {
        //_loader = loader;
       // print(22222);
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => _loader.LoadLevel(_config));
    }
}