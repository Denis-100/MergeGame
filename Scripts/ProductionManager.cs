using UnityEngine;
using System;
using System.Numerics;

public class ProductionManager : MonoBehaviour
{
    [HideInInspector] public float mineTimer = 0;
    [HideInInspector] public float smelterTimer = 0;
    [HideInInspector] public float coinIncomeTimer = 0;
    public float coinIncomeInterval = 1.0f;

    private float autoSaveTimer = 0f;

    void Start() => CalculateOfflineProgress();

    void Update()
    {
        HandleMining();
        HandleSmelting();
        HandleCoinIncome();
        HandleAutoSave();
    }

    void HandleMining()
    {
        if (Config.MineCount <= 0) return;
        mineTimer += Time.deltaTime;
        if (mineTimer >= Config.MineTimer)
        {
            Config.CopperCount += GameFormulas.GetMiningOutput();
            mineTimer = 0;
        }
    }

    void HandleSmelting()
    {
        if (Config.SmelterCount <= 0) return;
        smelterTimer += Time.deltaTime;

        if (smelterTimer >= Config.SmelterTimer)
        {
            // Сколько печка должна потребить за один цикл (базовое значение)
            BigInteger targetConsumption = GameFormulas.GetSmelterConsumption();

            if (Config.CopperCount >= targetConsumption)
            {
                // Случай 1: Ресурсов достаточно для полной работы
                Config.CopperCount -= targetConsumption;
                Config.CopperCoinCount += GameFormulas.GetSmelterOutput();
            }
            else if (Config.CopperCount > 0)
            {
                // Случай 2: Ресурсов мало, плавим «на остатки»
                // Вычисляем, сколько единиц продукции (монет) получится из имеющейся меди
                // Для этого делим текущую медь на стоимость производства одной единицы

                // Сначала найдем цену 1 монеты в меди:
                BigInteger costPerOneCoin = targetConsumption / GameFormulas.GetSmelterOutput();

                if (costPerOneCoin > 0)
                {
                    BigInteger possibleOutput = Config.CopperCount / costPerOneCoin;

                    if (possibleOutput > 0)
                    {
                        Config.CopperCount -= possibleOutput * costPerOneCoin;
                        Config.CopperCoinCount += possibleOutput;
                    }
                }
            }

            smelterTimer = 0;
            // Можно добавить SaveManager.SaveGame() здесь, если нужно сохранять прогресс печки
        }
    }

    void HandleCoinIncome()
    {
        coinIncomeTimer += Time.deltaTime;
        if (coinIncomeTimer >= coinIncomeInterval)
        {
            Config.RubCount += GetTotalCoinIncome() * (BigInteger)Config.GlobalMultiplier * Config.prestigeMultiCoinIncome;
            coinIncomeTimer = 0;
        }
    }

    public BigInteger GetTotalCoinIncome()
    {
        return Config.coinOlovoCount * Config.coinOlovoIncome + Config.coinBronzaCount * Config.coinBronzaIncome +
               Config.coinIronCount * Config.coinIronIncome + Config.coinSilverCount * Config.coinSilverIncome +
               Config.coinGoldCount * Config.coinGoldIncome + Config.coinTitanCount * Config.coinTitanIncome +
               Config.coinDiamondCount * Config.coinDiamondIncome + Config.coinObsidianCount * Config.coinObsidianIncome +
               Config.coinRybinCount * Config.coinRybinIncome + Config.coinSingularCount * Config.coinSingularIncome;
    }

    void CalculateOfflineProgress()
    {
        if (!PlayerPrefs.HasKey(SaveManager.LastSaveTimeKey)) return;
        long temp = Convert.ToInt64(PlayerPrefs.GetString(SaveManager.LastSaveTimeKey));
        DateTime lastTime = DateTime.FromBinary(temp);
        float secondsOffline = (float)(DateTime.Now - lastTime).TotalSeconds;

        if (secondsOffline < 10) return;

        // Шахты
        BigInteger mineCycles = (BigInteger)(secondsOffline / Config.MineTimer);
        Config.CopperCount += mineCycles * GameFormulas.GetMiningOutput();

        // Плавильни
        BigInteger smelterCycles = (BigInteger)(secondsOffline / Config.SmelterTimer);
        BigInteger costPerCycle = GameFormulas.GetSmelterConsumption();
        BigInteger finalSmelterCycles = (costPerCycle > 0) ? BigInteger.Min(smelterCycles, Config.CopperCount / costPerCycle) : 0;
        Config.CopperCount -= finalSmelterCycles * costPerCycle;
        Config.CopperCoinCount += finalSmelterCycles * GameFormulas.GetSmelterOutput();

        // Доход
        BigInteger incomeCycles = (BigInteger)(secondsOffline / coinIncomeInterval);
        Config.RubCount += GetTotalCoinIncome() * incomeCycles * (BigInteger)Config.GlobalMultiplier;

        SaveManager.SaveGame();
    }

    private void HandleAutoSave()
    {
        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer > 30)
        {
            SaveManager.SaveGame();
            autoSaveTimer = 0;
        }
    }
    private void OnApplicationPause(bool pause) { if (pause) SaveManager.SaveGame(); }
}