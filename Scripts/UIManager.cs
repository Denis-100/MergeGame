using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Баланс")]
    public TextMeshProUGUI copperText;
    public TextMeshProUGUI copperCoinText;
    public TextMeshProUGUI coinText;

    [Header("Прогресс-бары")]
    public Slider mineSlider;
    public Slider smelterSlider;

    [Header("Информация о доходах шахт и плавилен в Улучшениях")]
    public TextMeshProUGUI infoText;

    [Header("Текст заголовка Обмен")]
    public TextMeshProUGUI exchangeHeaderDeltaText;


    [Header("Текста кнопок клика")]
    public TextMeshProUGUI copperButtonText;
    public TextMeshProUGUI smelterButtonText;
    public TextMeshProUGUI coinButtonText;

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

    [Header("UI Тексты: УЛУЧШЕНИЯ КЛИКОВ")]
    public TextMeshProUGUI clickCopperLvlText;
    public TextMeshProUGUI clickCopperCostText;
    [Space]
    public TextMeshProUGUI clickCoinLvlText;
    public TextMeshProUGUI clickCoinCostText;
    [Space]
    public TextMeshProUGUI clickRubLvlText;
    public TextMeshProUGUI clickRubCostText;

    [Header("Карточки обмена (10 элементов)")]
    public TextMeshProUGUI[] coinCountTexts;
    public TextMeshProUGUI[] exchangeOneTopTexts;
    public TextMeshProUGUI[] exchangeOneBotTexts;
    public TextMeshProUGUI[] exchangeMaxTopTexts;
    public TextMeshProUGUI[] exchangeMaxBotTexts;

    [Header("Вкладка перерождение")]
    public TextMeshProUGUI textInfoPrestige;
    public TextMeshProUGUI textPricePrestige;

    [Space]
    public ProductionManager prodManager;

    private BigInteger tempPowerCopper, tempPowerCoin, tempPowerRub;

    private void Awake()
    {
        SaveManager.LoadGame();
        Application.targetFrameRate = 120;
    }

    private void Start() => UpdateButtonPowerText();

    void Update()
    {
        // Баланс
        if (copperText) copperText.text = $"Руда: {Formatter.FormatValue(Config.CopperCount)}";
        if (copperCoinText) copperCoinText.text = $"Монеты: {Formatter.FormatValue(Config.CopperCoinCount)}";
        if (coinText) coinText.text = $"Рублей: {Formatter.FormatValue(Config.RubCount)}";

        // Прогресс-бары
        if (mineSlider != null && Config.MineCount > 0) mineSlider.value = prodManager.mineTimer / Config.MineTimer;
        if (smelterSlider != null && Config.SmelterCount > 0) smelterSlider.value = prodManager.smelterTimer / Config.SmelterTimer;

        UpdateUpgradesUI();
        UpdateExchangeUI();
        UpdatePresigeUI();
        UpdateButtonPowerText();
        HandleClickPowerUpdates();
    }

    void UpdateUpgradesUI()
    {
        // Шахты
        if (mineInfoText) mineInfoText.text = $"Шахт: {Config.MineCount} | Сила: {Formatter.FormatValue(Config.MinePower)} | Скорость: {Config.MineTimer:F1}с";
        if (mineBuyCostText) mineBuyCostText.text = $"{Formatter.FormatValue(Config.MineBuyCost)} руб.";
        if (minePowerCostText) minePowerCostText.text = $"{Formatter.FormatValue(Config.MinePowerCost)} руб.";
        if (mineSpeedCostText) mineSpeedCostText.text = Config.MineTimer <= Config.MineMinTimer ? "МАКС." : $"{Formatter.FormatValue(Config.MineSpeedCost)} руб.";

        // Плавильни
        if (smelterInfoText) smelterInfoText.text = $"Печей: {Config.SmelterCount} | Сила: {Formatter.FormatValue(Config.SmelterPower)} | Скорость: {Config.SmelterTimer:F1}с";
        if (smelterBuyCostText) smelterBuyCostText.text = $"{Formatter.FormatValue(Config.SmelterBuyCost)} руб.";
        if (smelterPowerCostText) smelterPowerCostText.text = $"{Formatter.FormatValue(Config.SmelterPowerCost)} руб.";
        if (smelterSpeedCostText) smelterSpeedCostText.text = Config.SmelterTimer <= Config.SmelterMinTimer ? "МАКС." : $"{Formatter.FormatValue(Config.SmelterSpeedCost)} руб.";

        // Улучшения кликов
        if (clickCopperLvlText) clickCopperLvlText.text = $"Медь LVL: {Config.lvlClickCopper} | Сила: {Formatter.FormatValue(Config.ClickCopperPower)}";
        if (clickCopperCostText) clickCopperCostText.text = $"{Formatter.FormatValue(Config.CostClickCopper)} руб.";

        if (clickCoinLvlText) clickCoinLvlText.text = $"Плавка Монет LVL: {Config.lvlClickCoin} | Сила: {Formatter.FormatValue(Config.ClickCoinPower)}";
        if (clickCoinCostText) clickCoinCostText.text = $"{Formatter.FormatValue(Config.CostClickCoin)} руб.";

        if (clickRubLvlText) clickRubLvlText.text = $"Рубли LVL: {Config.lvlClickRub} | Сила: {Formatter.FormatValue(Config.ClickRubPower)}";
        if (clickRubCostText) clickRubCostText.text = $"{Formatter.FormatValue(Config.CostClickRub)} руб.";

        if (infoText)
        {
            infoText.text = $"Шахты: +{Formatter.FormatValue(Config.MineCount * Config.MinePower * (BigInteger)Config.GlobalMultiplier)} меди | " +
                            $"Печи: +{Formatter.FormatValue(Config.SmelterCount * Config.SmelterPower * (BigInteger)Config.GlobalMultiplier)} монеты";
        }
    }

    void UpdateExchangeUI()
    {
        float interval = prodManager != null ? prodManager.coinIncomeInterval : 1.0f;
        BigInteger totalIncome = 0;

        for (int i = 0; i < coinCountTexts.Length; i++)
        {
            if (coinCountTexts[i] == null) continue;
            int currentTier = i + 1;
            int prevTier = i;

            BigInteger currentCount = GetCoinCountByIndex(currentTier);
            BigInteger incomePerCoin = GetCoinIncomeByIndex(currentTier);
            BigInteger cardIncome = currentCount * incomePerCoin * (BigInteger)Config.GlobalMultiplier;
            totalIncome += cardIncome;

            coinCountTexts[i].text = $"{GetCoinNameByIndex(currentTier)} (+{Formatter.FormatValue(incomePerCoin)} руб/с) \nКоличество: {Formatter.FormatValue(currentCount)}";

            string prevCoinName = GetCoinShortNameByIndex(prevTier);
            BigInteger unitCost = (currentTier == 0) ? Config.CoinExchangeCost * 5 : Config.CoinExchangeCost * BigInteger.Pow(currentTier, 5) * 5;

            if (i < exchangeOneTopTexts.Length && exchangeOneTopTexts[i]) exchangeOneTopTexts[i].text = $"{Config.coinRate} {prevCoinName}";
            if (i < exchangeOneBotTexts.Length && exchangeOneBotTexts[i]) exchangeOneBotTexts[i].text = $"{Formatter.FormatValue(unitCost)} руб.";

            BigInteger possibleByCoins = GetCoinCountByIndex(prevTier) / Config.coinRate;
            BigInteger possibleByMoney = unitCost > 0 ? Config.RubCount / unitCost : possibleByCoins;
            BigInteger maxPossible = (possibleByCoins < possibleByMoney) ? possibleByCoins : possibleByMoney;

            if (i < exchangeMaxTopTexts.Length && exchangeMaxTopTexts[i]) exchangeMaxTopTexts[i].text = $"{Formatter.FormatValue(maxPossible * Config.coinRate)} {prevCoinName}";
            if (i < exchangeMaxBotTexts.Length && exchangeMaxBotTexts[i]) exchangeMaxBotTexts[i].text = $"{Formatter.FormatValue(maxPossible * unitCost)} руб.";
        }

        if (exchangeHeaderDeltaText) exchangeHeaderDeltaText.text = $"Общий доход: +{Formatter.FormatValue(totalIncome)} руб / {interval} сек";
    }

    void UpdatePresigeUI()
    {
        if (textInfoPrestige) textInfoPrestige.text = $"Перерождений: {Formatter.FormatValue(Config.prestigeCount)} | Очков ПР: {Formatter.FormatValue(Config.prestigePoints)}\nГлобальный множитель: {Config.GlobalMultiplier}";
        if (textPricePrestige) textPricePrestige.text = $"Цена: {Formatter.FormatValue(Config.prestigePrice)}";
    }

    private void HandleClickPowerUpdates()
    {
        if (tempPowerCopper != Config.ClickCopperPower) { tempPowerCopper = Config.ClickCopperPower; copperButtonText.text = $"+{Formatter.FormatValue(Config.ClickCopperPower * (BigInteger)Config.GlobalMultiplier)} Медь"; }
        if (tempPowerCoin != Config.ClickCoinPower) { tempPowerCoin = Config.ClickCoinPower; smelterButtonText.text = $"+{Formatter.FormatValue(Config.ClickCoinPower * (BigInteger)Config.GlobalMultiplier)} Монет"; }
        if (tempPowerRub != Config.ClickRubPower) { tempPowerRub = Config.ClickRubPower; coinButtonText.text = $"+{Formatter.FormatValue(Config.ClickRubPower * (BigInteger)Config.GlobalMultiplier)} Рублей"; }
    }

    public void UpdateButtonPowerText()
    {
        tempPowerCopper = Config.ClickCopperPower; tempPowerCoin = Config.ClickCoinPower; tempPowerRub = Config.ClickRubPower;
        copperButtonText.text = $"+{Formatter.FormatValue(Config.ClickCopperPower * (BigInteger)Config.GlobalMultiplier)} Медь";
        smelterButtonText.text = $"+{Formatter.FormatValue(Config.ClickCoinPower * (BigInteger)Config.GlobalMultiplier)} Монет";
        coinButtonText.text = $"+{Formatter.FormatValue(Config.ClickRubPower * (BigInteger)Config.GlobalMultiplier)} Рублей";
    }

    private BigInteger GetCoinCountByIndex(int index) { return index switch { 0 => Config.CopperCoinCount, 1 => Config.coinOlovoCount, 2 => Config.coinBronzaCount, 3 => Config.coinIronCount, 4 => Config.coinSilverCount, 5 => Config.coinGoldCount, 6 => Config.coinTitanCount, 7 => Config.coinDiamondCount, 8 => Config.coinObsidianCount, 9 => Config.coinRybinCount, 10 => Config.coinSingularCount, _ => 0 }; }
    private BigInteger GetCoinIncomeByIndex(int index) { return index switch { 0 => 0, 1 => Config.coinOlovoIncome, 2 => Config.coinBronzaIncome, 3 => Config.coinIronIncome, 4 => Config.coinSilverIncome, 5 => Config.coinGoldIncome, 6 => Config.coinTitanIncome, 7 => Config.coinDiamondIncome, 8 => Config.coinObsidianIncome, 9 => Config.coinRybinIncome, 10 => Config.coinSingularIncome, _ => 0 }; }
    private string GetCoinNameByIndex(int index) { string[] names = { "Медная монета", "Оловянная монета", "Бронзовая монета", "Железная монета", "Серебряная монета", "Золотая монета", "Титановая монета", "Алмазная монета", "Обсидиановая монета", "Рубиновая монета", "Сингулярность" }; return (index >= 0 && index < names.Length) ? names[index] : "Монета"; }
    private string GetCoinShortNameByIndex(int index) { string[] shortNames = { "мед. мон", "ол. мон", "брон. мон", "жел. мон", "сер. мон", "зол. мон", "тит. мон", "алм. мон", "обс. мон", "руб. мон", "синг" }; return (index >= 0 && index < shortNames.Length) ? shortNames[index] : "шт."; }
}