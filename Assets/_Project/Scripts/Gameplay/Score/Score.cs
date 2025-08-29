using System;

public class Score
{
    public event Action<int> OnValueEnd;
    public event Action<int> OnValueChange;
    
    public int Value { get; private set; }

    public int AdditionalValue { get; private set; } = 1;
    
    public int ValueForWin { get; private set; }

    public void SetValueForWin(int valueForWin) => ValueForWin = valueForWin;

    public void AddValue()
    {
        Value += AdditionalValue;
        
        OnValueChange?.Invoke(Value);
        
        if(Value >= ValueForWin)
            OnValueEnd?.Invoke(Value);
    }
    
    public void ReduceValue(int value = 2) => AdditionalValue *= value;
}