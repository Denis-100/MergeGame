using UnityEngine;
using UnityEngine.SceneManagement;
using System.Numerics;

public static class SaveManager
{
    // --- КЛЮЧИ ВРЕМЕНИ ---
    public const string LastSaveTimeKey = "LastSaveTime";

    // --- КЛЮЧИ РЕСУРСОВ ---
    private const string CopperCountKey = "CopperCount";
    private const string CopperCoinCountKey = "CopperCoinCount";
    private const string CoinCountKey = "RubCount";

    // --- КЛЮЧИ ШАХТ ---
    private const string MineCountKey = "MineCount";
    private const string MinePowerKey = "MinePower";
    private const string MineTimerKey = "MineTimer";
    private const string MineBuyCostKey = "MineBuyCost";
    private const string MinePowerCostKey = "MinePowerCost";
    private const string MineSpeedCostKey = "MineSpeedCost";

    // --- КЛЮЧИ ПЛАВИЛЕН ---
    private const string SmelterCountKey = "SmelterCount";
    private const string SmelterPowerKey = "SmelterPower";
    private const string SmelterTimerKey = "SmelterTimer";
    private const string SmelterBuyCostKey = "SmelterBuyCost";
    private const string SmelterPowerCostKey = "SmelterPowerCost";
    private const string SmelterSpeedCostKey = "SmelterSpeedCost";

    // --- КЛЮЧИ КЛИКОВ ---
    private const string ClickCopperPowerKey = "ClickCopperPower";
    private const string CostClickCopperKey = "CostClickCopper";
    private const string ClickCoinPowerKey = "ClickCoinPower";
    private const string CostClickCoinKey = "CostClickCoin";
    private const string ClickRubPowerKey = "ClickRubPower";
    private const string CostClickRubKey = "CostClickRub";

    // --- КЛЮЧИ УРОВНЕЙ КЛИКА ---
    private const string LvlClickCopperKey = "LvlClickCopper";
    private const string LvlClickCoinKey = "LvlClickCoin";
    private const string LvlClickRubKey = "LvlClickRub";

    // --- КЛЮЧИ ПЕРЕРОЖДЕНИЯ ---
    private const string PrestigePriceKey = "PrestigePrice";
    private const string PrestigeCountKey = "PrestigeCount";
    private const string PrestigeAddPointsKey = "PrestigeAddPoints";
    private const string PrestigePointsKey = "prestigePoints";
    private const string GlobalMultiplierKey = "GlobalMultiplier";
    private const string PrestigeMultiPointsKey = "PrestigeMultiPoints";
    private const string PrestigeMultiMineKey = "PrestigeMultiMine";
    private const string PrestigeMultiSmelterKey = "PrestigeMultiSmelter";
    private const string PrestigeMultiIncomeCoinKey = "PrestigeMultiIncomeCoin";
    private const string prestigeMultiPointsPriceKey = "prestigeMultiPointsPrice";
    private const string prestigeMultiMinePriceKey = "prestigeMultiMinePrice";
    private const string prestigeMultiSmelterPriceKey = "prestigeMultiSmelterPrice";
    private const string prestigeMultiCoinIncomePriceKey = "prestigeMultiCoinIncomePrice";


    // --- КЛЮЧИ МОНЕТ ---
    private const string CoinOlovoCountKey = "CoinOlovoCount";
    private const string CoinBronzaCountKey = "CoinBronzaCount";
    private const string CoinIronCountKey = "CoinIronCount";
    private const string CoinSilverCountKey = "CoinSilverCount";
    private const string CoinGoldCountKey = "CoinGoldCount";
    private const string CoinTitanCountKey = "CoinTitanCount";
    private const string CoinDiamondCountKey = "CoinDiamondCount";
    private const string CoinObsidianCountKey = "CoinObsidianCount";
    private const string CoinRybinCountKey = "CoinRybinCount";
    private const string CoinSingularCountKey = "CoinSingularCount";

    // Ключ для звука
    private static string MasterVolumeKey = "MasterVolume";
    private static string MasterMusicKey = "MasterMusicKey";

    // Ключ для панели помощи
    private static string IsFirstRunKey = "IsFirstRun";

    // Хелперы для BigInteger
    private static void SetBigInt(string key, BigInteger value) => PlayerPrefs.SetString(key, value.ToString());

    //private static BigInteger GetBigInt(string key, BigInteger defaultValue)
    //{
    //    string saved = PlayerPrefs.GetString(key, "");
    //    return string.IsNullOrEmpty(saved) ? defaultValue : BigInteger.Parse(saved);
    //}

    // Нужен только для переноса глобального множитнля из int в BigInteger
    private static BigInteger GetBigInt(string key, BigInteger defaultValue)
    {
        // 1. Проверяем, есть ли новое строковое значение (BigInt)
        if (PlayerPrefs.HasKey(key))
        {
            string saved = PlayerPrefs.GetString(key, "");
            if (!string.IsNullOrEmpty(saved))
            {
                return BigInteger.Parse(saved);
            }
        }

        // 2. Если строки нет, ищем старое int значение
        // Важно: мы не используем PlayerPrefs.HasKey здесь, так как HasKey 
        // возвращает true для любого типа под этим ключом.
        int legacyValue = PlayerPrefs.GetInt(key, -1);

        if (legacyValue != -1)
        {
            BigInteger convertedValue = new BigInteger(legacyValue);
            // Сразу конвертируем в новый формат, чтобы в следующий раз сработал пункт 1
            SetBigInt(key, convertedValue);
            return convertedValue;
        }

        return defaultValue;
    }


    // Сохранение
    public static void SaveGame()
    {
        // Время
        PlayerPrefs.SetString(LastSaveTimeKey, System.DateTime.Now.ToBinary().ToString());

        // Ресурсы
        SetBigInt(CopperCountKey, Config.CopperCount);
        SetBigInt(CopperCoinCountKey, Config.CopperCoinCount);
        SetBigInt(CoinCountKey, Config.RubCount);

        // Шахты
        PlayerPrefs.SetInt(MineCountKey, Config.MineCount);
        SetBigInt(MinePowerKey, Config.MinePower);
        PlayerPrefs.SetFloat(MineTimerKey, Config.MineTimer);
        SetBigInt(MineBuyCostKey, Config.MineBuyCost);
        SetBigInt(MinePowerCostKey, Config.MinePowerCost);
        SetBigInt(MineSpeedCostKey, Config.MineSpeedCost);

        // Плавильни
        PlayerPrefs.SetInt(SmelterCountKey, Config.SmelterCount);
        SetBigInt(SmelterPowerKey, Config.SmelterPower);
        PlayerPrefs.SetFloat(SmelterTimerKey, Config.SmelterTimer);
        SetBigInt(SmelterBuyCostKey, Config.SmelterBuyCost);
        SetBigInt(SmelterPowerCostKey, Config.SmelterPowerCost);
        SetBigInt(SmelterSpeedCostKey, Config.SmelterSpeedCost);

        // Клик
        SetBigInt(ClickCopperPowerKey, Config.ClickCopperPower);
        SetBigInt(CostClickCopperKey, Config.CostClickCopper);
        SetBigInt(ClickCoinPowerKey, Config.ClickCoinPower);
        SetBigInt(CostClickCoinKey, Config.CostClickCoin);
        SetBigInt(ClickRubPowerKey, Config.ClickRubPower);
        SetBigInt(CostClickRubKey, Config.CostClickRub);

        // Клик (сохранение уровней)
        PlayerPrefs.SetInt(LvlClickCopperKey, Config.lvlClickCopper);
        PlayerPrefs.SetInt(LvlClickCoinKey, Config.lvlClickCoin);
        PlayerPrefs.SetInt(LvlClickRubKey, Config.lvlClickRub);

        // Перерождение
        SetBigInt(PrestigePriceKey, Config.prestigePrice);
        PlayerPrefs.SetInt(PrestigeCountKey, Config.prestigeCount);
        SetBigInt(PrestigeAddPointsKey, Config.prestigeAddPoints);
        SetBigInt(PrestigePointsKey, Config.prestigePoints);
        SetBigInt(GlobalMultiplierKey, Config.GlobalMultiplier);
        SetBigInt(PrestigeMultiPointsKey, Config.prestigeMultiPoints);
        SetBigInt(PrestigeMultiMineKey, Config.prestigeMultiMine);
        SetBigInt(PrestigeMultiSmelterKey, Config.prestigeMultiSmelter);
        SetBigInt(PrestigeMultiIncomeCoinKey, Config.prestigeMultiCoinIncome);
        SetBigInt(prestigeMultiPointsPriceKey, Config.prestigeMultiPointsPrice);
        SetBigInt(prestigeMultiMinePriceKey, Config.prestigeMultiMinePrice);
        SetBigInt(prestigeMultiSmelterPriceKey, Config.prestigeMultiSmelterPrice);
        SetBigInt(prestigeMultiCoinIncomePriceKey, Config.prestigeMultiCoinIncomePrice);
        
        // Монеты
        SetBigInt(CoinOlovoCountKey, Config.coinOlovoCount);
        SetBigInt(CoinBronzaCountKey, Config.coinBronzaCount);
        SetBigInt(CoinIronCountKey, Config.coinIronCount);
        SetBigInt(CoinSilverCountKey, Config.coinSilverCount);
        SetBigInt(CoinGoldCountKey, Config.coinGoldCount);
        SetBigInt(CoinTitanCountKey, Config.coinTitanCount);
        SetBigInt(CoinDiamondCountKey, Config.coinDiamondCount);
        SetBigInt(CoinObsidianCountKey, Config.coinObsidianCount);
        SetBigInt(CoinRybinCountKey, Config.coinRybinCount);
        SetBigInt(CoinSingularCountKey, Config.coinSingularCount);

        // Звук
        PlayerPrefs.SetFloat(MasterVolumeKey, Config.MasterVolume);
        PlayerPrefs.SetFloat(MasterMusicKey, Config.MusicVolume);

        // Панель помощи
        PlayerPrefs.SetInt(IsFirstRunKey, Config.IsFirstRun);

        PlayerPrefs.Save();
    }

    // Загрузка
    public static void LoadGame()
    {
        // Ресурсы
        Config.CopperCount = GetBigInt(CopperCountKey, 0);
        Config.CopperCoinCount = GetBigInt(CopperCoinCountKey, 0);
        Config.RubCount = GetBigInt(CoinCountKey, 0);

        // Шахты
        Config.MineCount = PlayerPrefs.GetInt(MineCountKey, 0);
        Config.MinePower = GetBigInt(MinePowerKey, 1);
        Config.MineTimer = PlayerPrefs.GetFloat(MineTimerKey, 7.0f);
        Config.MineBuyCost = GetBigInt(MineBuyCostKey, 150);
        Config.MinePowerCost = GetBigInt(MinePowerCostKey, 200);
        Config.MineSpeedCost = GetBigInt(MineSpeedCostKey, 300);

        // Плавильни
        Config.SmelterCount = PlayerPrefs.GetInt(SmelterCountKey, 0);
        Config.SmelterPower = GetBigInt(SmelterPowerKey, 1);
        Config.SmelterTimer = PlayerPrefs.GetFloat(SmelterTimerKey, 7.0f);
        Config.SmelterBuyCost = GetBigInt(SmelterBuyCostKey, 150);
        Config.SmelterPowerCost = GetBigInt(SmelterPowerCostKey, 300);
        Config.SmelterSpeedCost = GetBigInt(SmelterSpeedCostKey, 500);

        // Клик
        Config.ClickCopperPower = GetBigInt(ClickCopperPowerKey, 1);
        Config.CostClickCopper = GetBigInt(CostClickCopperKey, 20);
        Config.ClickCoinPower = GetBigInt(ClickCoinPowerKey, 1);
        Config.CostClickCoin = GetBigInt(CostClickCoinKey, 50);
        Config.ClickRubPower = GetBigInt(ClickRubPowerKey, 1);
        Config.CostClickRub = GetBigInt(CostClickRubKey, 100);

        // Клик (загрузка уровней)
        Config.lvlClickCopper = PlayerPrefs.GetInt(LvlClickCopperKey, 1);
        Config.lvlClickCoin = PlayerPrefs.GetInt(LvlClickCoinKey, 1);
        Config.lvlClickRub = PlayerPrefs.GetInt(LvlClickRubKey, 1);

        // Перерождение
        Config.prestigePrice = GetBigInt(PrestigePriceKey, 100000000);
        Config.prestigeCount = PlayerPrefs.GetInt(PrestigeCountKey, 0);
        Config.prestigeAddPoints = GetBigInt(PrestigeAddPointsKey, 1);
        Config.prestigePoints = GetBigInt(PrestigePointsKey, 0);
        Config.GlobalMultiplier = GetBigInt(GlobalMultiplierKey, 1);
        Config.prestigeMultiPoints = GetBigInt(PrestigeMultiPointsKey, 1);
        Config.prestigeMultiMine = GetBigInt(PrestigeMultiMineKey, 1);
        Config.prestigeMultiSmelter = GetBigInt(PrestigeMultiSmelterKey, 1);
        Config.prestigeMultiCoinIncome = GetBigInt(PrestigeMultiIncomeCoinKey, 1);
        Config.prestigeMultiPointsPrice = GetBigInt(prestigeMultiPointsPriceKey, 1);
        Config.prestigeMultiMinePrice = GetBigInt(prestigeMultiMinePriceKey, 1);
        Config.prestigeMultiSmelterPrice = GetBigInt(prestigeMultiSmelterPriceKey, 1);
        Config.prestigeMultiCoinIncomePrice = GetBigInt(prestigeMultiCoinIncomePriceKey, 1);

        // Монеты
        Config.coinOlovoCount = GetBigInt(CoinOlovoCountKey, 0);
        Config.coinBronzaCount = GetBigInt(CoinBronzaCountKey, 0);
        Config.coinIronCount = GetBigInt(CoinIronCountKey, 0);
        Config.coinSilverCount = GetBigInt(CoinSilverCountKey, 0);
        Config.coinGoldCount = GetBigInt(CoinGoldCountKey, 0);
        Config.coinTitanCount = GetBigInt(CoinTitanCountKey, 0);
        Config.coinDiamondCount = GetBigInt(CoinDiamondCountKey, 0);
        Config.coinObsidianCount = GetBigInt(CoinObsidianCountKey, 0);
        Config.coinRybinCount = GetBigInt(CoinRybinCountKey, 0);
        Config.coinSingularCount = GetBigInt(CoinSingularCountKey, 0);

        // Звук
        Config.MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
        Config.MusicVolume = PlayerPrefs.GetFloat(MasterMusicKey, 0.2f);

        // Панель помощи
        Config.IsFirstRun = PlayerPrefs.GetInt(IsFirstRunKey, 0);

    }

    // Сброс
    public static void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Config.ResetData();

        SaveGame();

        ProductionManager prod = Object.FindFirstObjectByType<ProductionManager>();
        if (prod != null)
        {
            prod.mineTimer = 0;
            prod.smelterTimer = 0;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Прогресс сброшен");
    }

    // Сброс
    public static void PrestigeResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Config.ResetData();

        SaveGame();

        ProductionManager prod = Object.FindFirstObjectByType<ProductionManager>();
        if (prod != null)
        {
            prod.mineTimer = 0;
            prod.smelterTimer = 0;
        }
;
        Debug.Log("Сделано перерождение. Прогресс сброшен");
    }
}
