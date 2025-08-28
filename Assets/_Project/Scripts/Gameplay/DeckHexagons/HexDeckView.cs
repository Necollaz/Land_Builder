using System.Collections.Generic;
using UnityEngine;

public class HexDeckView
{
    private readonly Transform deckRoot;
    private readonly GameObjectPool<HexTileView> pool;

    private readonly List<HexTileView> visuals = new List<HexTileView>();

    private Vector3 baseLocalOffset;
    private Vector3 stackDirectionLocal;
    private Vector3 hiddenTilesLocalScale;
    private Vector3 topTilesLocalScale;
    private Quaternion tilesLocalRotation;

    private int visibleBelowTopCount;
    
    private float packedStep;
    private float top1ExtraStep;
    private float top2ExtraStep;
    private float top3ExtraStep;

    public HexDeckView(Transform deckRoot, GameObjectPool<HexTileView> pool, Vector3 baseLocalOffset, Vector3 stackDirectionLocal, Vector3 hiddenTilesLocalScale,
        Vector3 topTilesLocalScale, Quaternion tilesLocalRotation, float packedStep, float top1ExtraStep, float top2ExtraStep, float top3ExtraStep, int visibleBelowTopCount)
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
    }

    public void ShiftAfterDraw(int totalCount) => ApplyLayout(totalCount);
    
    public void RebuildVisuals(int totalCount, int ensureVisualsCount)
    {
        int topCount = Mathf.Min(3, totalCount);
        int startIndex = Mathf.Max(0, totalCount - (topCount + visibleBelowTopCount));
        int needVisuals = totalCount - startIndex;
        int want = Mathf.Max(needVisuals, ensureVisualsCount);

        while (visuals.Count < want)
            visuals.Add(pool.Take());

        while (visuals.Count > want)
        {
            HexTileView last = visuals[visuals.Count - 1];
            
            visuals.RemoveAt(visuals.Count - 1);
            pool.Return(last);
        }

        ApplyLayout(totalCount);
    }

    private void ApplyLayout(int totalCount)
    {
        int topCount = Mathf.Min(3, totalCount);
        int startIndex = Mathf.Max(0, totalCount - (topCount + visibleBelowTopCount));
        int visualSlots = Mathf.Min(visuals.Count, totalCount - startIndex);

        for (int i = 0; i < visualSlots; i++)
        {
            int sourceIndex = startIndex + i;
            Transform hexTransform = visuals[i].transform;
            
            hexTransform.SetParent(deckRoot, false);
            hexTransform.localRotation = tilesLocalRotation;

            float extra = 0f;
            
            if (totalCount >= 1 && sourceIndex == totalCount - 1)
                extra = top1ExtraStep;
            else if (totalCount >= 2 && sourceIndex == totalCount - 2)
                extra = top2ExtraStep;
            else if (totalCount >= 3 && sourceIndex == totalCount - 3)
                extra = top3ExtraStep;

            Vector3 localPosition = baseLocalOffset + stackDirectionLocal * (packedStep * sourceIndex + extra);
            hexTransform.localPosition = localPosition;

            bool isTop3 = (totalCount >= 1 && sourceIndex == totalCount - 1) || (totalCount >= 2 && sourceIndex == totalCount - 2) ||
                          (totalCount >= 3 && sourceIndex == totalCount - 3);

            hexTransform.localScale = isTop3 ? topTilesLocalScale : hiddenTilesLocalScale;
        }
        
        for (int i = visualSlots; i < visuals.Count; i++)
        {
            visuals[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < visualSlots; i++)
        {
            if (!visuals[i].gameObject.activeSelf)
                visuals[i].gameObject.SetActive(true);
        }
    }
}