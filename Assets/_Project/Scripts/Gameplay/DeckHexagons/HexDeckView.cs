using System.Collections.Generic;
using UnityEngine;

public class HexDeckView
{
    private const float PER_ITEM_STAGGER_SECONDS = 0.03f;
    
    private readonly Transform deckRoot;
    private readonly GameObjectPool<HexTileView> pool;
    
    private readonly List<HexTileView> visuals = new List<HexTileView>();
    private readonly List<HexTileData> snapshot = new List<HexTileData>();

    private readonly float shiftDurationSeconds;
    
    private Vector3 baseLocalOffset;
    private Vector3 stackDirectionLocal;
    private Vector3 hiddenTilesLocalScale;
    private Vector3 topTilesLocalScale;
    private Vector3 topAnchor;
    private Quaternion tilesLocalRotation;

    private int visibleBelowTopCount;
    
    private float packedStep;
    private float top1ExtraStep;
    private float top2ExtraStep;
    private float top3ExtraStep;
    
    private bool topAnchorInitialized;

    public HexDeckView(Transform deckRoot, GameObjectPool<HexTileView> pool, Vector3 baseLocalOffset, Vector3 stackDirectionLocal, Vector3 hiddenTilesLocalScale,
        Vector3 topTilesLocalScale, Quaternion tilesLocalRotation, float packedStep, float top1ExtraStep, float top2ExtraStep, float top3ExtraStep, int visibleBelowTopCount,
        float shiftDurationSeconds)
    {
        this.deckRoot = deckRoot;
        this.pool = pool;
        this.baseLocalOffset = baseLocalOffset;
        this.stackDirectionLocal = stackDirectionLocal.normalized;
        this.hiddenTilesLocalScale = hiddenTilesLocalScale;
        this.topTilesLocalScale = topTilesLocalScale;
        this.tilesLocalRotation = tilesLocalRotation;

        this.packedStep = Mathf.Max(0f, packedStep);
        this.top1ExtraStep = Mathf.Max(0f, top1ExtraStep);
        this.top2ExtraStep = Mathf.Max(0f, top2ExtraStep);
        this.top3ExtraStep = Mathf.Max(0f, top3ExtraStep);
        this.visibleBelowTopCount = Mathf.Max(0, visibleBelowTopCount);
        this.shiftDurationSeconds = Mathf.Max(0f, shiftDurationSeconds);
        
        topAnchorInitialized = false;
        topAnchor = baseLocalOffset;
    }

    public void SetSnapshot(HexTileData[] items)
    {
        snapshot.Clear();
        
        if (items != null && items.Length > 0)
            snapshot.AddRange(items);
        
        if (!topAnchorInitialized && snapshot.Count > 0)
        {
            topAnchor = baseLocalOffset + stackDirectionLocal * (packedStep * (snapshot.Count - 1));
            topAnchorInitialized = true;
        }
    }
    
    public void ShiftAfterDraw() => ApplyLayout(true);
    
    public void RefreshLayout() => ApplyLayout(animate: false);
    
    public void RebuildVisuals(int ensureVisualsCount)
    {
        int showCount = Mathf.Min(snapshot.Count, 3 + visibleBelowTopCount);
        int want = Mathf.Max(showCount, ensureVisualsCount);

        while (visuals.Count < want)
            visuals.Add(pool.Take());
        
        while (visuals.Count > want)
        {
            HexTileView last = visuals[^1];
            visuals.RemoveAt(visuals.Count - 1);
            pool.Return(last);
        }
        
        ApplyLayout(false);
    }
    
    private void ApplyLayout(bool animate)
    {
        int total = snapshot.Count;
        int showCount = Mathf.Min(total, 3 + visibleBelowTopCount);

        for (int i = 0; i < visuals.Count; i++)
        {
            bool active = i < showCount;
            HexTileView tileView = visuals[i];
            tileView.gameObject.SetActive(active);
            
            if (!active)
                continue;

            float extraNow = ExtraForIndex(i);
            Vector3 anchor = (topAnchorInitialized ? topAnchor : baseLocalOffset);

            float heightFromTopNow = -packedStep * i + extraNow;
            Vector3 targetPosition = anchor + stackDirectionLocal * heightFromTopNow;
            Vector3 targetScale = ScaleForIndex(i);
            
            Transform tileViewTransform = tileView.transform;
            tileViewTransform.SetParent(deckRoot, false);
            tileViewTransform.localRotation = tilesLocalRotation;

            tileView.ApplyData(snapshot[i]);

            if (animate && shiftDurationSeconds > 0f)
            {
                int previewIndex = i + 1;
                float extraPreview = ExtraForIndex(previewIndex);
                float heightFromTopPreview = -packedStep * previewIndex + extraPreview;
                Vector3 previewPosition = anchor + stackDirectionLocal * heightFromTopPreview;
                Vector3 previewScale = ScaleForIndex(previewIndex);
                
                tileView.SnapTo(previewPosition, previewScale, tilesLocalRotation);

                float delay = i * PER_ITEM_STAGGER_SECONDS;

                tileView.AnimateToDelayed(targetPosition, targetScale, tilesLocalRotation, shiftDurationSeconds, delay);
            }
            else
            {
                tileView.SnapTo(targetPosition, targetScale, tilesLocalRotation);
            }
        }
        
        for (int i = showCount; i < visuals.Count; i++)
            visuals[i].gameObject.SetActive(false);
    }
    
    private float ExtraForIndex(int index)
    {
        if (index == 0)
            return top1ExtraStep;
        
        if (index == 1)
            return top2ExtraStep;
        
        if (index == 2)
            return top3ExtraStep;
        
        return 0f;
    }

    private Vector3 ScaleForIndex(int index) => (index <= 2) ? topTilesLocalScale : hiddenTilesLocalScale;
}