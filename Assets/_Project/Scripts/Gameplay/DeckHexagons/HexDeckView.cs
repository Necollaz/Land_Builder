using System.Collections.Generic;
using UnityEngine;

public class HexDeckView
{
    private readonly Transform _deckRoot;
    private readonly GameObjectPool<HexTileView> _pool;

    private readonly List<HexTileView> _visuals = new List<HexTileView>();

    private readonly Vector3 _baseLocalOffset;
    private readonly Vector3 _stackDirectionLocal;
    private readonly Vector3 _hiddenTilesLocalScale;
    private readonly Vector3 _topTilesLocalScale;
    private readonly Quaternion _tilesLocalRotation;

    private readonly int _visibleTopCount;
    private readonly float _packedStep;
    private readonly float _visibleStep;

    public HexDeckView(Transform deckRoot, GameObjectPool<HexTileView> pool, Vector3 baseLocalOffset, Vector3 stackDirectionLocal, Vector3 hiddenTilesLocalScale,
        Vector3 topTilesLocalScale, Quaternion tilesLocalRotation, float packedStep, float visibleStep, int visibleTopCount)
    {
        _deckRoot = deckRoot;
        _pool = pool;
        _baseLocalOffset = baseLocalOffset;
        _stackDirectionLocal = stackDirectionLocal.normalized;
        _hiddenTilesLocalScale = hiddenTilesLocalScale;
        _topTilesLocalScale = topTilesLocalScale;
        _tilesLocalRotation = tilesLocalRotation;
        _packedStep = Mathf.Max(0f, packedStep);
        _visibleStep = Mathf.Max(0f, visibleStep);
        _visibleTopCount = Mathf.Max(0, visibleTopCount);
    }

    public void RebuildVisuals(int totalCount, int ensureVisualsCount)
    {
        int want = Mathf.Max(ensureVisualsCount, Mathf.Min(totalCount, _visibleTopCount + 1));

        while (_visuals.Count < want)
            _visuals.Add(_pool.Take());

        while (_visuals.Count > want)
        {
            HexTileView last = _visuals[_visuals.Count - 1];
            
            _visuals.RemoveAt(_visuals.Count - 1);
            _pool.Return(last);
        }

        ApplyLayout(totalCount);
    }

    public void ShiftAfterDraw(int totalCount)
    {
        ApplyLayout(totalCount);
    }

    private void ApplyLayout(int totalCount)
    {
        int hiddenCount = Mathf.Max(0, totalCount - _visibleTopCount);

        for (int i = 0; i < _visuals.Count; i++)
        {
            HexTileView view = _visuals[i];
            Transform viewTransform = view.transform;

            viewTransform.SetParent(_deckRoot, false);
            viewTransform.localRotation = _tilesLocalRotation;

            bool isHiddenLayer = i == 0;

            if (isHiddenLayer)
            {
                viewTransform.localScale = _hiddenTilesLocalScale;
                Vector3 position = _baseLocalOffset + _stackDirectionLocal * (_packedStep * hiddenCount);
                viewTransform.localPosition = position;
            }
            else
            {
                int visibleIndexFromBottom = i - 1;
                float basePackedHeight = _packedStep * hiddenCount;
                float addVisibleHeight = _visibleStep * visibleIndexFromBottom;
                Vector3 position = _baseLocalOffset + _stackDirectionLocal * (basePackedHeight + addVisibleHeight);

                viewTransform.localScale = _topTilesLocalScale;
                viewTransform.localPosition = position;
            }
        }
    }
}