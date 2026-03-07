using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using System;

public class UpgradesManager : MonoBehaviour
{
    [Header("Прогресс-бары")]
    public Slider mineSlider;
    public Slider smelterSlider;

    [Header("Информация о доходаъ шахт и плавилен")]
    public TextMeshProUGUI infoText;

    [Header("UI Тексты: ШАХТЫ")]
    public TextMeshProUGUI mineInfoText;
    public TextMeshProUGUI mineBuyCostText;
    public TextMeshProUGUI minePowerCostText;
    public TextMeshProUGUI mineSpeedCostText;

    [Header("UI Тексты: ПЛАВИЛЬНИ")]
    public TextMeshProUGUI smelterInfoText;
    public TextMeshProUGUI smelterBuyCostText;
    public TextMeshProUGUI smelterPowerCostText;
    public TextMeshProUGUI smelterSpeedCostText;

    [Header("UI Тексты: КЛИКИ КОЛИЧЕСТВО ПОКУПКИ")]
    public TextMeshProUGUI clickCopperBuyCountText;
    public TextMeshProUGUI clickCoinBuyCountText;
    public TextMeshProUGUI clickRubBuyCountText;

    [Header("UI Тексты: КЛИКИ СТОИМОСТЬ")]
    public TextMeshProUGUI clickCopperCostText;
    public TextMeshProUGUI clickCoinCostText;
    public TextMeshProUGUI clickRubCostText;

    [Header("UI Тексты: КЛИКИ ИНФОРМАЦИЯ")]
    public TextMeshProUGUI clickCopperText;
    public TextMeshProUGUI clickCoinText;
    public TextMeshProUGUI clickRubText;

    private ProductionManager production;

    // Сколько силы клика прибавиться за улучшение
    private BigInteger addPowerCopper = 1;
    private BigInteger addPowerCoin = 1;
    private BigInteger addPowerRub = 1;

    private void Start()
    {
        production = UnityEngine.Object.FindFirstObjectByType<ProductionManager>();
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Отображение данных Шахт
        if (mineInfoText) mineInfoText.text = $"Шахт: {Config.MineCount} | Сила: {Formatter.FormatValue(Config.MinePower)} | Скорость: {Config.MineTimer:F1}с";
        // Цены
        if (mineBuyCostText) mineBuyCostText.text = $"{Formatter.FormatValue(Config.MineBuyCost)} руб.";
        if (minePowerCostText) minePowerCostText.text = $"{Formatter.FormatValue(Config.MinePowerCost)} руб.";
        if (mineSpeedCostText)
        {
            if (Config.MineTimer <= Config.MineMinTimer)
                mineSpeedCostText.text = "МАКС.";
            else
                mineSpeedCostText.text = $"{Formatter.FormatValue(Config.MineSpeedCost)} руб.";
        }

        // Отображение данных Плавилен
        if (smelterInfoText) smelterInfoText.text = $"Печей: {Config.SmelterCount} | Сила: {Formatter.FormatValue(Config.SmelterPower)} | Скорость: {Config.SmelterTimer:F1}с";
        // Цены
        if (smelterBuyCostText) smelterBuyCostText.text = $"{Formatter.FormatValue(Config.SmelterBuyCost)} руб.";
        if (smelterPowerCostText) smelterPowerCostText.text = $"{Formatter.FormatValue(Config.SmelterPowerCost)} руб.";
        if (smelterSpeedCostText)
        {
            if (Config.SmelterTimer <= Config.SmelterMinTimer)
                smelterSpeedCostText.text = "МАКС.";
            else
                smelterSpeedCostText.text = $"{Formatter.FormatValue(Config.SmelterSpeedCost)} руб.";
        }

        // Отображение сколько силы прибавиться к клику
        if (clickCopperBuyCountText) clickCopperBuyCountText.text = $"КУПИТЬ +{Formatter.FormatValue(addPowerCopper)}";
        if (clickCoinBuyCountText) clickCoinBuyCountText.text = $"КУПИТЬ +{Formatter.FormatValue(addPowerCoin)}";
        if (clickRubBuyCountText) clickRubBuyCountText.text = $"КУПИТЬ +{Formatter.FormatValue(addPowerRub)}";

        // Отображение цен Кликов
        if (clickCopperCostText) clickCopperCostText.text = $"{Formatter.FormatValue(Config.CostClickCopper)} руб.";
        if (clickCoinCostText) clickCoinCostText.text = $"{Formatter.FormatValue(Config.CostClickCoin)} руб.";
        if (clickRubCostText) clickRubCostText.text = $"{Formatter.FormatValue(Config.CostClickRub)} руб.";

        // Прогресс-бар
        if (mineSlider != null && Config.MineCount > 0) mineSlider.value = production.mineTimer / Config.MineTimer;
        if (smelterSlider != null && Config.SmelterCount > 0) smelterSlider.value = production.smelterTimer / Config.SmelterTimer;

        // Сила клика
        if (clickCopperText) clickCopperText.text = $"LVL: {Config.lvlClickCopper} | Руды за клик: {Formatter.FormatValue(Config.ClickCopperPower)}";
        if (clickCoinText) clickCoinText.text = $"LVL: {Config.lvlClickCoin} | Монет за клик: {Formatter.FormatValue(Config.ClickCoinPower)}";
        if (clickRubText) clickRubText.text = $"LVL: {Config.lvlClickRub} | Рублей за клик: {Formatter.FormatValue(Config.ClickRubPower)}";

        // Информация о доходах
        if (infoText) infoText.text = $"Шахты: +{Formatter.FormatValue(Config.MineCount * Config.MinePower * Config.GlobalMultiplier)} меди | " +
                                      $"Печи: +{Formatter.FormatValue(Config.SmelterCount * Config.SmelterPower * Config.GlobalMultiplier)} монеты";
    }

    // --- ЛОГИКА ШАХТ ---

    public void BuyMine()
    {
        if (Config.RubCount >= Config.MineBuyCost)
        {
            Config.RubCount -= Config.MineBuyCost;
            Config.MineCount++;
            Config.MineBuyCost = (Config.MineBuyCost * (BigInteger)(Config.MultiMineBuy * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    public void UpgradeMinePower()
    {
        if (Config.RubCount >= Config.MinePowerCost)
        {
            Config.RubCount -= Config.MinePowerCost;
            Config.MinePower += Config.MineAddPower;
            Config.MinePowerCost = (Config.MinePowerCost * (BigInteger)(Config.MultiMinePower * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    public void UpgradeMineSpeed()
    {
        if (Config.RubCount >= Config.MineSpeedCost && Config.MineTimer > Config.MineMinTimer)
        {
            Config.RubCount -= Config.MineSpeedCost;
            Config.MineTimer -= 0.1f;
            Config.MineSpeedCost = (Config.MineSpeedCost * (BigInteger)(Config.MultiMineSpeed * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    // --- ЛОГИКА ПЛАВИЛЕН ---

    public void BuySmelter()
    {
        if (Config.RubCount >= Config.SmelterBuyCost)
        {
            Config.RubCount -= Config.SmelterBuyCost;
            Config.SmelterCount++;
            Config.SmelterBuyCost = (Config.SmelterBuyCost * (BigInteger)(Config.MultiSmelterBuy * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    public void UpgradeSmelterPower()
    {
        if (Config.RubCount >= Config.SmelterPowerCost)
        {
            Config.RubCount -= Config.SmelterPowerCost;
            Config.SmelterPower++;
            Config.SmelterPowerCost = (Config.SmelterPowerCost * (BigInteger)(Config.MultiSmelterPower * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    public void UpgradeSmelterSpeed()
    {
        if (Config.RubCount >= Config.SmelterSpeedCost && Config.SmelterTimer > Config.SmelterMinTimer)
        {
            Config.RubCount -= Config.SmelterSpeedCost;
            Config.SmelterTimer -= 0.1f;
            Config.SmelterSpeedCost = (Config.SmelterSpeedCost * (BigInteger)(Config.MultiSmelterSpeed * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    // --- ЛОГИКА КЛИКОВ ---

    public void UpgradeClickCopper()
    {
        if (Config.RubCount >= Config.CostClickCopper)
        {
            Config.RubCount -= Config.CostClickCopper;
            Config.lvlClickCopper++;

            BigInteger baseStep = 1;

            BigInteger expGrowth = (BigInteger)Math.Pow(Config.growthRateCopper, Config.lvlClickCopper);

            addPowerCopper = baseStep + expGrowth;

            addPowerCopper = (addPowerCopper * (BigInteger)(Config.MultiClickCopperPower * 100)) / 100;

            Config.ClickCopperPower += addPowerCopper;
            Config.CostClickCopper = (Config.CostClickCopper * (BigInteger)(Config.MultiClickCopper * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    public void UpgradeClickCoin()
    {
        if (Config.RubCount >= Config.CostClickCoin)
        {
            Config.RubCount -= Config.CostClickCoin;
            Config.lvlClickCoin++;

            BigInteger baseStep = 1;
            
            BigInteger expGrowth = (BigInteger)Math.Pow(Config.growthRateCoin, Config.lvlClickCoin);

            addPowerCoin = baseStep + expGrowth;

            // Применяем множитель из конфига
            addPowerCoin = (addPowerCoin * (BigInteger)(Config.MultiClickCoinPower * 100)) / 100;

            Config.ClickCoinPower += addPowerCoin;
            Config.CostClickCoin = (Config.CostClickCoin * (BigInteger)(Config.MultiClickCoin * 100)) / 100;
            SaveManager.SaveGame();
        }
    }

    public void UpgradeClickRub()
    {
        if (Config.RubCount >= Config.CostClickRub)
        {
            Config.RubCount -= Config.CostClickRub;
            Config.lvlClickRub++;

            BigInteger baseStep = 1;

            BigInteger expGrowth = (BigInteger)Math.Pow(Config.growthRateRub, Config.lvlClickRub);

            addPowerRub = baseStep + expGrowth;

            addPowerRub = (addPowerRub * (BigInteger)(Config.MultiClickRubPower * 100)) / 100;

            Config.ClickRubPower += addPowerRub;

            // Цена растет (убедись, что MultiClickRub не слишком большой, обычно 1.15-1.5)
            Config.CostClickRub = (Config.CostClickRub * (BigInteger)(Config.MultiClickRub * 100)) / 100;

            SaveManager.SaveGame();
        }
    }
}