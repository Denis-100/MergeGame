using UnityEngine;
using System.Numerics;

public class Exchange : MonoBehaviour
{
    // Метод для обмена монет текущего типа на 1 монету следующего типа
    public void ExchangeOne(int index)
    {
        BigInteger unitCost = GameFormulas.GetExchangeMoneyCost(index + 1);

        // Проверяем, хватает ли монет и денег
        if (GetCoinCount(index) >= Config.coinRate && Config.RubCount >= unitCost)
        {
            Config.RubCount -= unitCost;
            ChangeCoinCount(index, -Config.coinRate);     // Забираем 10 (или coinRate) штук
            ChangeCoinCount(index + 1, 1);                // Даем 1 штуку следующего тира
            SaveManager.SaveGame();
        }
    }

    // Метод для обмена максимально возможного количества
    public void ExchangeMax(int index)
    {
        BigInteger unitCost = GameFormulas.GetExchangeMoneyCost(index + 1);

        // Используем общую формулу расчета максимального кол-ва
        BigInteger finalExchanges = GameFormulas.CalculateMaxExchange(
            GetCoinCount(index),
            Config.RubCount,
            Config.coinRate,
            unitCost
        );

        if (finalExchanges > 0)
        {
            Config.RubCount -= finalExchanges * unitCost;
            ChangeCoinCount(index, -(finalExchanges * Config.coinRate));
            ChangeCoinCount(index + 1, finalExchanges);
            SaveManager.SaveGame();
        }
    }

    // Вспомогательный метод получения количества монет по индексу (используется для расчетов)
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
