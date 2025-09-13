using UnityEngine;

public class PlayerPrefsCurrencyWallet : ICurrencyWallet
{
    private const string COINS_KEY = "player_coins";

    public void AddCoins(int amount)
    {
        int current = PlayerPrefs.GetInt(COINS_KEY, 0);
        long sum = (long)current + amount;
        int clamped = (int)Mathf.Clamp(sum, 0, int.MaxValue);
        
        PlayerPrefs.SetInt(COINS_KEY, clamped);
        PlayerPrefs.Save();
    }
}