public class ScoreGiver
{
    private Score _score;

    public ScoreGiver(Score score)
    {
        _score = score;
    }

    public void AddScore(HexType newType, HexType existType)
    {
        if (newType == existType)
        {
            _score.AddValue();
        }
    }
}
