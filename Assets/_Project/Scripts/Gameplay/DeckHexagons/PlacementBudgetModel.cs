using UnityEngine;

public class PlacementBudgetModel
{
    public delegate void IntChanged(int value);
    
    public event IntChanged BudgetChanged;

    public int Remaining { get; private set; }

    public PlacementBudgetModel(int start)
    {
        Remaining = Mathf.Max(0, start);
    }

    public bool TrySpendOne()
    {
        if (Remaining <= 0)
            return false;

        Remaining--;
        
        BudgetChanged?.Invoke(Remaining);
        
        return true;
    }

    public void Add(int amount)
    {
        if (amount <= 0)
            return;
        
        Remaining += amount;
        
        BudgetChanged?.Invoke(Remaining);
    }
}