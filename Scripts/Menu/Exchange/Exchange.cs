using UnityEngine;
using System;
using System.Numerics;

public class Exchange : MonoBehaviour
{
    // Метод для обмена 10 монет текущего типа на 1 монету следующего типа
    // index: 0 - Олово, 1 - Бронза, 2 - Железо и т.д.
    public void ExchangeOne(int index)
    {
        // Проверяем, хватает ли монет и денег на операцию
        if (GetCoinCount(index) >= Config.coinRate && Config.RubCount >= GetExchangeCost(index))
        {
            Config.RubCount -= GetExchangeCost(index);
            ChangeCoinCount(index, -Config.coinRate);     // Забираем 10 штук
            ChangeCoinCount(index + 1, 1);                // Даем 1 штуку (следующий тир)
            SaveManager.SaveGame();
        }
    }

    // Метод для обмена максимально возможного количества
    public void ExchangeMax(int index)
    {
        BigInteger availableCoins = GetCoinCount(index);
        BigInteger possibleByCoins = availableCoins / Config.coinRate;

        if (possibleByCoins <= 0) return;

        BigInteger finalExchanges = possibleByCoins;
        BigInteger unitCost = GetExchangeCost(index);

        // Если обмен платный, проверяем лимит по рублям
        if (unitCost > 0)
        {
            BigInteger affordableByMoney = Config.RubCount / unitCost;
            if (affordableByMoney < finalExchanges) finalExchanges = affordableByMoney;
        }

        if (finalExchanges > 0)
        {
            Config.RubCount -= finalExchanges * unitCost;
            ChangeCoinCount(index, -(finalExchanges * Config.coinRate));
            ChangeCoinCount(index + 1, finalExchanges);
            SaveManager.SaveGame();
        }
    }

    // Вспомогательный метод расчета цены обмена
    private BigInteger GetExchangeCost(int index)
    {
        if (index == 0) return Config.CoinExchangeCost * 5;
        // Используем BigInteger.Pow для точных расчетов больших цен
        return Config.CoinExchangeCost * BigInteger.Pow(index, 5) * 5;
    }

    // Вспомогательный метод получения количества монет по индексу
    private BigInteger GetCoinCount(int index)
    {
        return index switch
        {
            0 => Config.CopperCoinCount,
            1 => Config.coinOlovoCount,
            2 => Config.coinBronzaCount,
            3 => Config.coinIronCount,
            4 => Config.coinSilverCount,
            5 => Config.coinGoldCount,
            6 => Config.coinTitanCount,
            7 => Config.coinDiamondCount,
            8 => Config.coinObsidianCount,
            9 => Config.coinRybinCount,
            10 => Config.coinSingularCount,
            _ => 0
        };
    }

    // Вспомогательный метод изменения количества монет по индексу
    private void ChangeCoinCount(int index, BigInteger amount)
    {
        switch (index)
        {
            case 0: Config.CopperCoinCount += amount; break;
            case 1: Config.coinOlovoCount += amount; break;
            case 2: Config.coinBronzaCount += amount; break;
            case 3: Config.coinIronCount += amount; break;
            case 4: Config.coinSilverCount += amount; break;
            case 5: Config.coinGoldCount += amount; break;
            case 6: Config.coinTitanCount += amount; break;
            case 7: Config.coinDiamondCount += amount; break;
            case 8: Config.coinObsidianCount += amount; break;
            case 9: Config.coinRybinCount += amount; break;
            case 10: Config.coinSingularCount += amount; break;
        }
    }
}
