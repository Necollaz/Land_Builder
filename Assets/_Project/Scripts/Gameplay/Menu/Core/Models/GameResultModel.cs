public class GameResultModel
{
    public delegate void GameResultChanged(GameResultType result);
    
    public event GameResultChanged ResultChanged;

    public GameResultType CurrentResult { get; private set; } = GameResultType.None;

    public void SetVictory()
    {
        if (CurrentResult == GameResultType.Victory)
            return;

        CurrentResult = GameResultType.Victory;
        
        ResultChanged?.Invoke(CurrentResult);
    }

    public void SetDefeat()
    {
        if (CurrentResult == GameResultType.Defeat)
            return;

        CurrentResult = GameResultType.Defeat;
        
        ResultChanged?.Invoke(CurrentResult);
    }

    public void ResetResult()
    {
        if (CurrentResult == GameResultType.None)
            return;

        CurrentResult = GameResultType.None;
        
        ResultChanged?.Invoke(CurrentResult);
    }
}