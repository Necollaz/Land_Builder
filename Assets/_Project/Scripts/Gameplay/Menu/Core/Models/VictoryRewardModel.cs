public class VictoryRewardModel
{
    public delegate void IntChanged(int value);
    
    public event IntChanged RewardDoubled;

    public int BaseCoins { get; private set; }
    public bool Doubled { get; private set; }

    public int FinalCoins => Doubled ? BaseCoins * 2 : BaseCoins;

    public void SetBaseCoins(int coins)
    {
        BaseCoins = coins < 0 ? 0 : coins;
        Doubled = false;
    }

    public void ApplyDouble()
    {
        if (Doubled)
            return;
        
        Doubled = true;
        
        RewardDoubled?.Invoke(FinalCoins);
    }
}