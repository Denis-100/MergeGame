using UnityEngine;
using System;
using System.Numerics;

public class ProductionManager : MonoBehaviour
{
    [HideInInspector] public float mineTimer = 0;
    [HideInInspector] public float smelterTimer = 0;
    [HideInInspector] public float coinIncomeTimer = 0; // Таймер дохода от монет

    public float coinIncomeInterval = 1.0f; // Раз во сколько секунд монеты дают рубли

    void Start()
    {
        CalculateOfflineProgress();
    }

    void Update()
    {
        HandleMining();
        HandleSmelting();
        HandleCoinIncome(); // Вызов дохода от монет
    }

    // Подсчет добычи меди с шахт
    void HandleMining()
    {
        if (Config.MineCount <= 0) return;

        mineTimer += Time.deltaTime;
        if (mineTimer >= Config.MineTimer)
        {
            Config.CopperCount += Config.MineCount * Config.MinePower * (BigInteger)Config.GlobalMultiplier;
            mineTimer = 0;
        }
    }

    // Подсчет дохода плавилен
    void HandleSmelting()
    {
        if (Config.SmelterCount <= 0) return;

        smelterTimer += Time.deltaTime;
        if (smelterTimer >= Config.SmelterTimer)
        {
            BigInteger totalCost = (BigInteger)(Config.SmelterCount * Config.SmelterPower * Config.MeltingCost * (BigInteger)Config.GlobalMultiplier);
            if (Config.CopperCount >= totalCost)
            {
                Config.CopperCount -= totalCost;
                Config.CopperCoinCount += Config.SmelterCount * Config.SmelterPower * (BigInteger)Config.GlobalMultiplier;
            }
            smelterTimer = 0;
        }
    }

    // Подсчет пассивного дохода от ценных монет в рубли
    void HandleCoinIncome()
    {
        coinIncomeTimer += Time.deltaTime;
        if (coinIncomeTimer >= coinIncomeInterval)
        {
            Config.RubCount += GetTotalCoinIncome() * (BigInteger)Config.GlobalMultiplier;
            coinIncomeTimer = 0;
        }
    }

    // Функция для получения суммарного дохода от всех типов монет
    public BigInteger GetTotalCoinIncome()
    {
        BigInteger total = 0;
        total += Config.coinOlovoCount * Config.coinOlovoIncome;
        total += Config.coinBronzaCount * Config.coinBronzaIncome;
        total += Config.coinIronCount * Config.coinIronIncome;
        total += Config.coinSilverCount * Config.coinSilverIncome;
        total += Config.coinGoldCount * Config.coinGoldIncome;
        total += Config.coinTitanCount * Config.coinTitanIncome;
        total += Config.coinDiamondCount * Config.coinDiamondIncome;
        total += Config.coinObsidianCount * Config.coinObsidianIncome;
        total += Config.coinRybinCount * Config.coinRybinIncome;
        total += Config.coinSingularCount * Config.coinSingularIncome;
        return total;
    }

    // Подсчет офлайн прогресса
    void CalculateOfflineProgress()
    {
        if (!PlayerPrefs.HasKey(SaveManager.LastSaveTimeKey)) return;

        long temp = Convert.ToInt64(PlayerPrefs.GetString(SaveManager.LastSaveTimeKey));
        DateTime lastTime = DateTime.FromBinary(temp);
        TimeSpan difference = DateTime.Now - lastTime;
        float secondsOffline = (float)difference.TotalSeconds;

        if (secondsOffline < 10) return; // Меньше 10 секунд не считаем

        // Шахты
        if (Config.MineCount > 0)
        {
            BigInteger cycles = (BigInteger)(secondsOffline / Config.MineTimer);
            BigInteger offlineCopper = cycles * Config.MineCount * Config.MinePower * (BigInteger)Config.GlobalMultiplier;
            Config.CopperCount += offlineCopper;
            Debug.Log($"Офлайн: шахты принесли {Formatter.FormatValue(offlineCopper)} руды");
        }

        // Плавильни
        if (Config.SmelterCount > 0)
        {
            BigInteger cycles = (BigInteger)(secondsOffline / Config.SmelterTimer);
            BigInteger costPerCycle = (BigInteger)(Config.SmelterCount * Config.MeltingCost);
            BigInteger possibleCycles = Config.CopperCount / costPerCycle;
            BigInteger finalCycles = (cycles < possibleCycles) ? cycles : possibleCycles;

            Config.CopperCount -= finalCycles * costPerCycle;
            Config.CopperCoinCount += finalCycles * Config.SmelterCount * Config.SmelterPower * (BigInteger)Config.GlobalMultiplier;
            Debug.Log($"Офлайн: плавильни сработали {finalCycles} раз");
        }

        // Пассивный доход от монет за офлайн
        BigInteger coinCycles = (BigInteger)(secondsOffline / coinIncomeInterval);
        if (coinCycles > 0)
        {
            Config.RubCount += GetTotalCoinIncome() * coinCycles * (BigInteger)Config.GlobalMultiplier;
            Debug.Log($"Офлайн: монеты принесли доход за {coinCycles} циклов");
        }

        SaveManager.SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveManager.SaveGame();
    }
}
