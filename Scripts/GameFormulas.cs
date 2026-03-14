using System;
using System.Numerics;

public static class GameFormulas
{
    // --- ЦЕНЫ УЛУЧШЕНИЙ ---
    public static BigInteger GetNextCost(BigInteger currentCost, float multiplier)
    {
        return (currentCost * (BigInteger)(multiplier * 100)) / 100;
    }

    // --- СИЛА КЛИКА (ЭКСПОНЕНТА) ---
    public static BigInteger CalculateClickUpgradePower(int lvl, float growthRate, float pwrMulti)
    {
        BigInteger expGrowth = (BigInteger)Math.Pow(growthRate, lvl);
        BigInteger addPower = 1 + expGrowth;
        return (addPower * (BigInteger)(pwrMulti * 100)) / 100;
    }

    // --- СИЛА КЛИКА С УЧЕТОМ МНОЖИТЕЛЕЙ ---
    public static BigInteger GetTotalClickCopper() => Config.ClickCopperPower * (BigInteger)Config.GlobalMultiplier;
    public static BigInteger GetTotalClickCoin() => Config.ClickCoinPower * (BigInteger)Config.GlobalMultiplier;
    public static BigInteger GetTotalClickRub() => Config.ClickRubPower * (BigInteger)Config.GlobalMultiplier;

    // --- ПРОИЗВОДСТВО ---
    public static BigInteger GetMiningOutput()
    {
        return Config.MineCount * Config.MinePower * (BigInteger)Config.GlobalMultiplier * Config.prestigeMultiMine;
    }

    public static BigInteger GetSmelterOutput()
    {
        return Config.SmelterCount * Config.SmelterPower * (BigInteger)Config.GlobalMultiplier * Config.prestigeMultiSmelter;
    }

    public static BigInteger GetSmelterConsumption()
    {
        return (BigInteger)(Config.SmelterCount * Config.SmelterPower * Config.MeltingCost * (BigInteger)Config.GlobalMultiplier * Config.prestigeMultiSmelter);
    }

    // --- ОБМЕН МОНЕТ ---
    public static BigInteger GetExchangeMoneyCost(int targetTier)
    {
        // Если медная монета
        if (targetTier < 1) targetTier = 1;

        // 3 возводиться в степерь тира монеты
        BigInteger expoGrowth = BigInteger.Pow(3, targetTier);

        // Дополнительно возводим тир в 3 степерь
        BigInteger tierWeight = BigInteger.Pow(targetTier, 3);

        // Возвращаем произведение всего для цены
        return Config.CoinExchangeCost * expoGrowth * tierWeight;
    }

    public static BigInteger CalculateMaxExchange(BigInteger prevCoinCount, BigInteger currentMoney, BigInteger rate, BigInteger moneyCost)
    {
        BigInteger possibleByCoins = prevCoinCount / rate;
        BigInteger possibleByMoney = moneyCost > 0 ? currentMoney / moneyCost : possibleByCoins;
        return (possibleByCoins < possibleByMoney) ? possibleByCoins : possibleByMoney;
    }

    public static BigInteger GetTotalExchangeMoneyCost(BigInteger amount, BigInteger unitCost)
    {
        return amount * unitCost;
    }

    public static BigInteger GetTotalExchangeCoinsCost(BigInteger amount, BigInteger rate)
    {
        return amount * rate;
    }
}