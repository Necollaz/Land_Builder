using System;
using System.Collections.Generic;

[Serializable]
public class HexTileData
{
    private const int MAX_SIDES = 6;
    
    public HexType[] SideTypes { get; private set; } = new HexType[6];
    
    public void RandomizeSides(int distinctTypesAllowed = 2, float continueSameChance = 0.6f, Random random = null)
    {
        random ??= new Random();

        int typeCount = Enum.GetNames(typeof(HexType)).Length;

        HexType primary = (HexType)random.Next(0, typeCount);
        HexType secondary = primary;

        if (distinctTypesAllowed >= 2)
        {
            do
                secondary = (HexType)random.Next(0, typeCount);
            while
                (secondary == primary && typeCount > 1);
        }

        SideTypes[0] = primary;
        
        for (int i = 1; i < MAX_SIDES; i++)
        {
            bool continueSame = random.NextDouble() <= continueSameChance;
            
            if (distinctTypesAllowed >= 2)
                SideTypes[i] = continueSame ? SideTypes[i - 1] : (SideTypes[i - 1] == primary ? secondary : primary);
            else
                SideTypes[i] = SideTypes[i - 1];
        }

        if (distinctTypesAllowed >= 2)
        {
            var distinct = new HashSet<HexType>(SideTypes);
            
            if (distinct.Count == 1)
            {
                int idx = random.Next(0, MAX_SIDES);
                
                SideTypes[idx] = secondary;
            }
        }
    }
}