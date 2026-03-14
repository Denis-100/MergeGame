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

    [Header("Информация о доходах")]
    public TextMeshProUGUI infoText;
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
    public TextMeshProUGUI clickCoinLvlText;
    public TextMeshProUGUI clickCoinCostText;
    public TextMeshProUGUI clickRubLvlText;
    public TextMeshProUGUI clickRubCostText;

    [Header("Карточки обмена (10 элементов)")]
    public TextMeshProUGUI[] coinCountTexts;
    public TextMeshProUGUI[] exchangeOneTopTexts;
    public TextMeshProUGUI[] exchangeOneBotTexts;
    public TextMeshProUGUI[] exchangeMaxTopTexts;
    public TextMeshProUGUI[] exchangeMaxBotTexts;

    [Header("Вкладка перерождение")]
    [SerializeField] private Slider prestigeSlider;
    public TextMeshProUGUI textInfoPrestige;
    public TextMeshProUGUI textPricePrestige;
    public TextMeshProUGUI textInfoPointsUpPrise;
    public TextMeshProUGUI textInfoPoints;
    public TextMeshProUGUI textInfoUpMinePrise;
    public TextMeshProUGUI textInfoUpMine;
    public TextMeshProUGUI textInfoUpSmelterPrise;
    public TextMeshProUGUI textInfoUpSmelter;
    public TextMeshProUGUI textInfoUpCoinIncomePrise;
    public TextMeshProUGUI textInfoUpCoinIncome;

    [Space]
    public ProductionManager prodManager;

    private BigInteger tempPowerCopper, tempPowerCoin, tempPowerRub;

    private void Awake()
    {
        SaveManager.LoadGame();
        Application.targetFrameRate = 120;

        VolumeSettings settings = Resources.FindObjectsOfTypeAll<VolumeSettings>()[0];
        if (settings) settings.Initialize();
    }

    private void Start() => UpdateButtonPowerText();

    void Update()
    {
        // Баланс
        if (copperText) copperText.text = $"Руда: {Formatter.FormatValue(Config.CopperCount)}";
        if (copperCoinText) copperCoinText.text = $"Монеты: {Formatter.FormatValue(Config.CopperCoinCount)}";
        if (coinText) coinText.text = $"Рублей: {Formatter.FormatValue(Config.RubCount)}";

        // Прогресс-бары
        if (mineSlider != null && Config.MineCount > 0)
        {
            if (Config.MineTimer < 0.1) mineSlider.value = 1;
            else mineSlider.value = prodManager.mineTimer / Config.MineTimer;
        }
        if (smelterSlider != null && Config.SmelterCount > 0)
        {
            if (Config.SmelterTimer < 0.1) smelterSlider.value = 1;
            else smelterSlider.value = prodManager.smelterTimer / Config.SmelterTimer;
        }
        UpdateUpgradesUI();
        UpdateExchangeUI();
        UpdatePresigeUI();
        HandleClickPowerUpdates();
        UpdatePrestigeSlider();
    }

    void UpdateUpgradesUI()
    {
        // Шахты
        if (mineInfoText) mineInfoText.text = $"Шахт: {Config.MineCount} | Сила: {Formatter.FormatValue(Config.MinePower)} | Скорость: {Config.MineTimer:F1}с";
        if (mineBuyCostText) mineBuyCostText.text = $"{Formatter.FormatValue(Config.MineBuyCost)} р.";
        if (minePowerCostText) minePowerCostText.text = $"{Formatter.FormatValue(Config.MinePowerCost)} р.";
        if (mineSpeedCostText) mineSpeedCostText.text = Config.MineTimer <= Config.MineMinTimer ? "МАКС." : $"{Formatter.FormatValue(Config.MineSpeedCost)} р.";

        // Плавильни
        if (smelterInfoText) smelterInfoText.text = $"Печей: {Config.SmelterCount} | Сила: {Formatter.FormatValue(Config.SmelterPower)} | Скорость: {Config.SmelterTimer:F1}с";
        if (smelterBuyCostText) smelterBuyCostText.text = $"{Formatter.FormatValue(Config.SmelterBuyCost)} р.";
        if (smelterPowerCostText) smelterPowerCostText.text = $"{Formatter.FormatValue(Config.SmelterPowerCost)} р.";
        if (smelterSpeedCostText) smelterSpeedCostText.text = Config.SmelterTimer <= Config.SmelterMinTimer ? "МАКС." : $"{Formatter.FormatValue(Config.SmelterSpeedCost)} р.";

        // Улучшения кликов
        if (clickCopperLvlText) clickCopperLvlText.text = $"Медь LVL: {Config.lvlClickCopper} | Сила: {Formatter.FormatValue(Config.ClickCopperPower)}";
        if (clickCopperCostText) clickCopperCostText.text = $"{Formatter.FormatValue(Config.CostClickCopper)} р.";

        if (clickCoinLvlText) clickCoinLvlText.text = $"Плавка Монет LVL: {Config.lvlClickCoin} | Сила: {Formatter.FormatValue(Config.ClickCoinPower)}";
        if (clickCoinCostText) clickCoinCostText.text = $"{Formatter.FormatValue(Config.CostClickCoin)} р.";

        if (clickRubLvlText) clickRubLvlText.text = $"Рубли LVL: {Config.lvlClickRub} | Сила: {Formatter.FormatValue(Config.ClickRubPower)}";
        if (clickRubCostText) clickRubCostText.text = $"{Formatter.FormatValue(Config.CostClickRub)} р.";

        if (infoText)
        {
            infoText.text = $"Шахты: +{Formatter.FormatValue(GameFormulas.GetMiningOutput())} меди | " +
                            $"Печи: +{Formatter.FormatValue(GameFormulas.GetSmelterOutput())} монеты";
        }
    }

    void UpdateExchangeUI()
    {
        float interval = prodManager != null ? prodManager.coinIncomeInterval : 1.0f;
        BigInteger totalIncome = 0;

        for (int i = 0; i < coinCountTexts.Length; i++)
        {
            if (coinCountTexts[i] == null) continue;
            int currentTier = i + 1; // Олово = 1, Бронза = 2...
            int prevTier = i;

            BigInteger currentCount = GetCoinCountByIndex(currentTier);
            BigInteger incomePerCoin = GetCoinIncomeByIndex(currentTier);
            BigInteger cardIncome = currentCount * incomePerCoin * (BigInteger)Config.GlobalMultiplier * Config.prestigeMultiCoinIncome;
            totalIncome += cardIncome;

            coinCountTexts[i].text = $"{GetCoinNameByIndex(currentTier)} (+{Formatter.FormatValue(incomePerCoin)} руб/с) \nКоличество: {Formatter.FormatValue(currentCount)}";

            BigInteger unitCost = GameFormulas.GetExchangeMoneyCost(currentTier);
            BigInteger prevCoinBalance = GetCoinCountByIndex(prevTier);

            // Расчет Max Buy
            BigInteger maxPossible = GameFormulas.CalculateMaxExchange(prevCoinBalance, Config.RubCount, Config.coinRate, unitCost);

            // Вывод цен (используем формулы!)
            BigInteger totalCoinsToSpend = GameFormulas.GetTotalExchangeCoinsCost(maxPossible, Config.coinRate);
            BigInteger totalMoneyToSpend = GameFormulas.GetTotalExchangeMoneyCost(maxPossible, unitCost);

            if (i < exchangeOneTopTexts.Length && exchangeOneTopTexts[i])
                exchangeOneTopTexts[i].text = $"{Config.coinRate} {GetCoinShortNameByIndex(i)}";

            if (i < exchangeOneBotTexts.Length && exchangeOneBotTexts[i])
                exchangeOneBotTexts[i].text = $"{Formatter.FormatValue(unitCost)} руб.";

            if (i < exchangeMaxTopTexts.Length && exchangeMaxTopTexts[i])
                exchangeMaxTopTexts[i].text = $"{Formatter.FormatValue(totalCoinsToSpend)} {GetCoinShortNameByIndex(prevTier)}";

            if (i < exchangeMaxBotTexts.Length && exchangeMaxBotTexts[i])
                exchangeMaxBotTexts[i].text = $"{Formatter.FormatValue(totalMoneyToSpend)} руб.";
        }

        // Общий заголовок дохода
        if (exchangeHeaderDeltaText)
            exchangeHeaderDeltaText.text = $"Общий доход: +{Formatter.FormatValue(totalIncome)} руб / {interval:F1} сек";
    }

    void UpdatePresigeUI()
    {
        if (textInfoPrestige) textInfoPrestige.text = $"Перерождений: {Formatter.FormatValue(Config.prestigeCount)} |" +
                $" Очков <sprite name=\"CoinPrestige_0\">: {Formatter.FormatValue(Config.prestigePoints)}\nГлобальный множитель: {Formatter.FormatValue(Config.GlobalMultiplier)}";
        if (textPricePrestige) textPricePrestige.text = $"{Formatter.FormatValue(Config.prestigePrice)} руб.";
        if (textInfoPointsUpPrise) textInfoPointsUpPrise.text = $"{Formatter.FormatValue(Config.prestigeMultiPointsPrice)} <sprite name=\"CoinPrestige_0\">";
        if (textInfoPoints) textInfoPoints.text = $"Множитель очков ПР: X{Formatter.FormatValue(Config.prestigeMultiPoints)}";
        if (textInfoUpMinePrise) textInfoUpMinePrise.text = $"{Formatter.FormatValue(Config.prestigeMultiMinePrice)} <sprite name=\"CoinPrestige_0\">";
        if (textInfoUpMine) textInfoUpMine.text = $"Множитель шахты: X{Formatter.FormatValue(Config.prestigeMultiMine)}";
        if (textInfoUpSmelterPrise) textInfoUpSmelterPrise.text = $"{Formatter.FormatValue(Config.prestigeMultiSmelterPrice)} <sprite name=\"CoinPrestige_0\">";
        if (textInfoUpSmelter) textInfoUpSmelter.text = $"Множитель плавилен: X{Formatter.FormatValue(Config.prestigeMultiSmelter)}";
        if (textInfoUpCoinIncomePrise) textInfoUpCoinIncomePrise.text = $"{Formatter.FormatValue(Config.prestigeMultiCoinIncomePrice)} <sprite name=\"CoinPrestige_0\">";
        if (textInfoUpCoinIncome) textInfoUpCoinIncome.text = $"Множитель дохода монет: X{Formatter.FormatValue(Config.prestigeMultiCoinIncome)}";
    }

    private void UpdatePrestigeSlider()
    {
        // Если денег 0 или меньше, слайдер в начале
        if (Config.RubCount <= 1)
        {
            prestigeSlider.value = 0f;
            return;
        }

        // Вычисляем логарифм текущего баланса и целевой цены
        // Log10(1000) вернет 3, Log10(1,000,000) вернет 6 и т.д.
        double currentLog = BigInteger.Log10(Config.RubCount);
        double targetLog = BigInteger.Log10(Config.prestigePrice);

        // Делим количество текущих "нулей" на целевое количество "нулей"
        float progress = (float)(currentLog / targetLog);

        // Ограничиваем от 0 до 1 на всякий случай
        prestigeSlider.value = Mathf.Clamp01(progress);
    }

    // Вспомогательный метод для точности деления логарифмов
    private double exp(double x) => System.Math.Exp(x);
    private void HandleClickPowerUpdates()
    {
        if (tempPowerCopper != Config.ClickCopperPower) { tempPowerCopper = Config.ClickCopperPower; UpdateButtonPowerText(); }
        if (tempPowerCoin != Config.ClickCoinPower) { tempPowerCoin = Config.ClickCoinPower; UpdateButtonPowerText(); }
        if (tempPowerRub != Config.ClickRubPower) { tempPowerRub = Config.ClickRubPower; UpdateButtonPowerText(); }
    }

    public void UpdateButtonPowerText()
    {
        tempPowerCopper = Config.ClickCopperPower;
        tempPowerCoin = Config.ClickCoinPower;
        tempPowerRub = Config.ClickRubPower;

        if (copperButtonText)
            copperButtonText.text = $"+{Formatter.FormatValue(GameFormulas.GetTotalClickCopper())} Медь";

        if (smelterButtonText)
            smelterButtonText.text = $"+{Formatter.FormatValue(GameFormulas.GetTotalClickCoin())} Монет";

        if (coinButtonText)
            coinButtonText.text = $"+{Formatter.FormatValue(GameFormulas.GetTotalClickRub())} Рублей";
    }

    private BigInteger GetCoinCountByIndex(int index) { return index switch { 0 => Config.CopperCoinCount, 1 => Config.coinOlovoCount, 2 => Config.coinBronzaCount, 3 => Config.coinIronCount, 4 => Config.coinSilverCount, 5 => Config.coinGoldCount, 6 => Config.coinTitanCount, 7 => Config.coinDiamondCount, 8 => Config.coinObsidianCount, 9 => Config.coinRybinCount, 10 => Config.coinSingularCount, _ => 0 }; }
    private BigInteger GetCoinIncomeByIndex(int index) { return index switch { 0 => 0, 1 => Config.coinOlovoIncome, 2 => Config.coinBronzaIncome, 3 => Config.coinIronIncome, 4 => Config.coinSilverIncome, 5 => Config.coinGoldIncome, 6 => Config.coinTitanIncome, 7 => Config.coinDiamondIncome, 8 => Config.coinObsidianIncome, 9 => Config.coinRybinIncome, 10 => Config.coinSingularIncome, _ => 0 }; }
    private string GetCoinNameByIndex(int index) { string[] names = { "Медная монета", "Оловянная монета", "Бронзовая монета", "Железная монета", "Серебряная монета", "Золотая монета", "Титановая монета", "Алмазная монета", "Обсидиановая монета", "Рубиновая монета", "Сингулярность" }; return (index >= 0 && index < names.Length) ? names[index] : "Монета"; }
    private string GetCoinShortNameByIndex(int index) { string[] shortNames = { "мед. мон", "ол. мон", "брон. мон", "жел. мон", "сер. мон", "зол. мон", "тит. мон", "алм. мон", "обс. мон", "руб. мон", "синг" }; return (index >= 0 && index < shortNames.Length) ? shortNames[index] : "шт."; }
}