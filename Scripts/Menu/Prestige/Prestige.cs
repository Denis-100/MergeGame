using UnityEngine;
using System.Numerics;

public class Prestige : MonoBehaviour
{
    public void buyPrestige()
    {
        if (Config.RubCount >= Config.prestigePrice)
        {
            Config.prestigePrice *= Config.prestigeMultiPrice;

            Config.prestigeCount++;
            Config.prestigePoints += Config.prestigeAddPoints;

            Config.GlobalMultiplier += Config.prestigeAddMulti;

            SaveManager.SaveGame();
            BigInteger prestigePrice = Config.prestigePrice;
            int prestigeCount = Config.prestigeCount;
            BigInteger prestigeAddPoints = Config.prestigeAddPoints;
            BigInteger prestigePoints = Config.prestigePoints;
            int GlobalMultiplier = Config.GlobalMultiplier;

            SaveManager.ResetGame();

            Config.prestigePrice = prestigePrice;
            Config.prestigeCount = prestigeCount;
            Config.prestigeAddPoints = prestigeAddPoints;
            Config.prestigePoints = prestigePoints;
            Config.GlobalMultiplier = GlobalMultiplier;

            SaveManager.SaveGame();
        }
    }
}
