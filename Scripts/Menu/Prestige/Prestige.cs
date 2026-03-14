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
            Config.prestigePoints += Config.prestigeAddPoints * Config.prestigeMultiPoints;

            Config.GlobalMultiplier *= Config.prestigeAddMultiGlobal;

            SaveManager.SaveGame();
            BigInteger prestigePrice = Config.prestigePrice;
            int prestigeCount = Config.prestigeCount;
            BigInteger prestigeAddPoints = Config.prestigeAddPoints;
            BigInteger prestigePoints = Config.prestigePoints;
            BigInteger GlobalMultiplier = Config.GlobalMultiplier;
            BigInteger prestigeMultiPoints = Config.prestigeMultiPoints;
            BigInteger prestigeMultiMine = Config.prestigeMultiMine;
            BigInteger prestigeMultiSmelter = Config.prestigeMultiSmelter;
            BigInteger prestigeMultiCoinIncome = Config.prestigeMultiCoinIncome;
            BigInteger prestigeMultiPointsPrice = Config.prestigeMultiPointsPrice;
            BigInteger prestigeMultiMinePrice = Config.prestigeMultiMinePrice;
            BigInteger prestigeMultiSmelterPrice = Config.prestigeMultiSmelterPrice;
            BigInteger prestigeMultiCoinIncomePrice = Config.prestigeMultiCoinIncomePrice;
            int IsFirstRun = Config.IsFirstRun;

            float volume = Config.MasterVolume;
            float MusicVolume = Config.MusicVolume;

            string lastVersion = PlayerPrefs.GetString("LastSeenVersion", "0.0");

            SaveManager.PrestigeResetGame();

            Config.prestigePrice = prestigePrice;
            Config.prestigeCount = prestigeCount;
            Config.prestigeAddPoints = prestigeAddPoints;
            Config.prestigePoints = prestigePoints;
            Config.GlobalMultiplier = GlobalMultiplier;
            Config.prestigeMultiPoints = prestigeMultiPoints;
            Config.prestigeMultiMine = prestigeMultiMine;
            Config.prestigeMultiSmelter = prestigeMultiSmelter;
            Config.prestigeMultiCoinIncome = prestigeMultiCoinIncome;
            Config.prestigeMultiPointsPrice = prestigeMultiPointsPrice;
            Config.prestigeMultiMinePrice = prestigeMultiMinePrice;
            Config.prestigeMultiSmelterPrice = prestigeMultiSmelterPrice;
            Config.prestigeMultiCoinIncomePrice = prestigeMultiCoinIncomePrice;

            Config.MasterVolume = volume;
            Config.MusicVolume = MusicVolume;

            Config.IsFirstRun = IsFirstRun;

            PlayerPrefs.SetString("LastSeenVersion", lastVersion);

            SaveManager.SaveGame();

        }
    }

    public void BuyPrestigeUpPoints() => TryUpgrade(ref Config.prestigeMultiPointsPrice, ref Config.prestigeMultiPoints, ref Config.prestigeAddMultiPoints);

    public void BuyPrestigeUpMine() => TryUpgrade(ref Config.prestigeMultiMinePrice, ref Config.prestigeMultiMine, ref Config.prestigeAddMultiMine);

    public void BuyPrestigeUpSmelter() => TryUpgrade(ref Config.prestigeMultiSmelterPrice, ref Config.prestigeMultiSmelter, ref Config.prestigeAddMultiSmelter);

    public void BuyPrestigeUpCoinIncome() => TryUpgrade(ref Config.prestigeMultiCoinIncomePrice, ref Config.prestigeMultiCoinIncome, ref Config.prestigeAddMultiCoinIncome);

    private void TryUpgrade(ref BigInteger price, ref BigInteger multi, ref BigInteger addMulti)
    {
        if (Config.prestigePoints >= price)
        {
            Config.prestigePoints -= price;

            multi *= addMulti;

            if (price < 2) price += 1;
            else price = (price * 1618) / 1000; // Рост по прогрессии чисел фибоначи


            SaveManager.SaveGame();
        }
    }
}
