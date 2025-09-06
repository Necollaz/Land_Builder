using UniRx;

public class Score
{
    public Subject<int> OnValueEnd = new ();
    
    public ReactiveProperty<int> Value = new ();

    public int AdditionalValue { get; private set; } = 1;
    
    public int ValueForWin { get; private set; }

    public void SetValueForWin(int valueForWin) => ValueForWin = valueForWin;

    public void AddValue()
    {
        Value.Value += AdditionalValue;
        
        if(Value.Value >= ValueForWin)
            OnValueEnd?.OnNext(Value.Value);
    }
    
    public void ReduceValue(int value = 2) => AdditionalValue *= value;
}