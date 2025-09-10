using System;
using System.Collections.Generic;

[Serializable]
public class HexTileData
{
    public HexType[] SideTypes { get; private set; } = new HexType[6];

    public void RandomizeSides(int distinctTypesAllowed = 2, float continueSameChance = 0.6f, Random rng = null)
    {
        rng = new Random();

        int typeCount = Enum.GetNames(typeof(HexType)).Length;

        HexType primary = (HexType)rng.Next(0, typeCount);
        HexType secondary = primary;

        if (distinctTypesAllowed >= 2)
        {
            do
                secondary = (HexType)rng.Next(0, typeCount);
            while
                (secondary == primary && typeCount > 1);
        }

        SideTypes[0] = primary;
        for (int i = 1; i < 6; i++)
        {
            if (distinctTypesAllowed >= 2 && rng.NextDouble() > continueSameChance)
                SideTypes[i] = (SideTypes[i - 1].Equals(primary) ? secondary : primary);
            else
                SideTypes[i] = SideTypes[i - 1];
        }

        if (distinctTypesAllowed >= 2)
        {
            var distinct = new HashSet<HexType>(SideTypes);
            
            if (distinct.Count == 1)
            {
                int idx = rng.Next(0, 6);
                
                SideTypes[idx] = secondary;
            }
        }
    }
}