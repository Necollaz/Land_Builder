using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelMapController : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private LevelNodeView _nodePrefab;
    [SerializeField] private LineRenderer _linePrefab;

    private IProgressService _progress;
    private LevelSelectionController _selection;

    private Dictionary<int, LevelNodeView> _nodes = new();
    
    public void Construct(IProgressService progress, LevelSelectionController selection)
    {
        _progress = progress;
        _selection = selection;
    }

    public void BuildMap(LevelConfig[] allLevels)
    {
        // Чистим предыдущие узлы
        foreach (Transform child in _container)
            Destroy(child.gameObject);

        _nodes.Clear();

        // Создаём узлы
        foreach (var config in allLevels)
        {
            var node = Instantiate(_nodePrefab, _container);
            bool unlocked = _progress.IsLevelUnlocked(config.LevelId);
            node.Initialize(config, unlocked, async c => await _selection.SelectLevel(c));

            _nodes[config.LevelId] = node;

            // Позиции можно брать из LevelConfig (например, Vector2 Position)
            node.GetComponent<RectTransform>().anchoredPosition = new Vector2(config.LevelId * 200, 0);
        }

        // Соединяем узлы линиями
        foreach (var config in allLevels)
        {
            if (!_nodes.TryGetValue(config.LevelId, out var fromNode))
                continue;

            foreach (var next in config.NextLevels)
            {
                if (_nodes.TryGetValue(next.LevelId, out var toNode))
                {
                    var line = Instantiate(_linePrefab, _container);
                    line.positionCount = 2;
                    line.useWorldSpace = false;
                    line.SetPosition(0, fromNode.GetComponent<RectTransform>().anchoredPosition);
                    line.SetPosition(1, toNode.GetComponent<RectTransform>().anchoredPosition);
                }
            }
        }
    }
}
