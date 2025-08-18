using System;
using System.Collections.Generic;

public class HexDeckModel
{
    private readonly Queue<HexTileData> queue = new Queue<HexTileData>();
    private readonly Random random;
    
    private readonly int maxSize;
    private readonly int refillBatch;
    private readonly int distinctTypesAllowed;
    
    private readonly float continueSameChance;

    public HexDeckModel(int maxSize, int refillBatch, int distinctTypesAllowed, float continueSameChance, Random random)
    {
        this.maxSize = Math.Max(1, maxSize);
        this.refillBatch = Math.Max(1, refillBatch);
        this.distinctTypesAllowed = Math.Max(1, distinctTypesAllowed);
        this.continueSameChance = Math.Clamp(continueSameChance, 0f, 1f);
        this.random = random ?? new Random();

        RefillIfNeeded(true);
    }
    
    public int Count => queue.Count;

    public HexTileData Draw()
    {
        if (queue.Count == 0)
            RefillIfNeeded(true);
        
        HexTileData data = queue.Dequeue();
        
        RefillIfNeeded(false);
        
        return data;
    }

    private void RefillIfNeeded(bool force)
    {
        if (!force && queue.Count > refillBatch)
            return;
        
        int target = Math.Max(maxSize, queue.Count + refillBatch);

        while (queue.Count < target)
        {
            HexTileData data = new HexTileData();
            
            data.RandomizeSides(distinctTypesAllowed, continueSameChance, random);
            queue.Enqueue(data);
        }
    }
}