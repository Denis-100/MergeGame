using UnityEngine;
using TMPro;
using System;
using System.Numerics;

public class UIManager : MonoBehaviour
{
    [Header("Баланс")]
    public TextMeshProUGUI copperText;
    public TextMeshProUGUI copperCoinText;
    public TextMeshProUGUI coinText;

    [Header("Текст заголовка Обмен")]
    public TextMeshProUGUI exchangeHeaderDeltaText;

    [Header("Текста кнопок клика")]
    public TextMeshProUGUI copperButtonText;
    public TextMeshProUGUI smelterButtonText;
    public TextMeshProUGUI coinButtonText;

    [Header("Карточки обмена (10 элементов)")]
    public TextMeshProUGUI[] coinCountTexts;       // Заголовок карточки
    public TextMeshProUGUI[] coinIncomeDeltaTexts;  // Доход карточки

    [Header("Тексты кнопок Обмена")]
    public TextMeshProUGUI[] exchangeOneTopTexts;  // Цена в монетах (верх)
    public TextMeshProUGUI[] exchangeOneBotTexts;  // Цена в рублях (низ)
    public TextMeshProUGUI[] exchangeMaxTopTexts;  // Цена макс в монетах (верх)
    public TextMeshProUGUI[] exchangeMaxBotTexts;  // Цена макс в рублях (низ)

    [Header("Вкладка перерождение")]
    public TextMeshProUGUI textInfoPrestige;        // Информация о престиже
    public TextMeshProUGUI textPricePrestige;      // Цена престижа

    [Space]
    public ProductionManager prodManager;

    private BigInteger tempPowerCopper;
    private BigInteger tempPowerCoin;
    private BigInteger tempPowerRub;

    private void Awake()
    {
        SaveManager.LoadGame();
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        UpdateStaticTexts();
    }

    void Update()
    {
        // Основной баланс сверху экрана
        if (copperText != null) copperText.text = $"Руда: {Formatter.FormatValue(Config.CopperCount)}";
        if (copperCoinText != null) copperCoinText.text = $"Монеты: {Formatter.FormatValue(Config.CopperCoinCount)}";
        if (coinText != null) coinText.text = $"Рублей: {Formatter.FormatValue(Config.RubCount)}";

        HandleClickPowerUpdates();
        UpdateExchangeUI();
        UpdatePresigeUI();
        UpdateStaticTexts();
    }

    void UpdatePresigeUI()
    {
        textInfoPrestige.text = $"Перерождений: {Formatter.FormatValue(Config.prestigeCount)} | " +
            $"Очков ПР: {Formatter.FormatValue(Config.prestigePoints)}\n" +
            $"Глобальный множитель: {Config.GlobalMultiplier}";

        textPricePrestige.text = $"Цена: {Formatter.FormatValue(Config.presigePrice)}";
    }

    // Интерфейс кнопок обмена монет
    void UpdateExchangeUI()
    {
        float interval = prodManager != null ? prodManager.coinIncomeInterval : 1.0f;
        BigInteger totalIncome = 0;

        for (int i = 0; i < coinCountTexts.Length; i++)
        {
            if (coinCountTexts[i] == null || i >= coinIncomeDeltaTexts.Length) continue;

            int currentTier = i + 1;
            int prevTier = i;

            BigInteger currentCount = GetCoinCountByIndex(currentTier);
            BigInteger incomePerCoin = GetCoinIncomeByIndex(currentTier);
            BigInteger cardIncome = currentCount * incomePerCoin * (BigInteger)Config.GlobalMultiplier;
            totalIncome += cardIncome;

            coinCountTexts[i].text = $"{GetCoinNameByIndex(currentTier)} (+{Formatter.FormatValue(incomePerCoin)} руб/с) \nКоличество: {Formatter.FormatValue(currentCount)}";

            if (coinIncomeDeltaTexts[i] != null)
                coinIncomeDeltaTexts[i].text = $"+{Formatter.FormatValue(cardIncome)} руб / {interval} сек";

            string prevCoinName = GetCoinShortNameByIndex(prevTier);

            // Кнопка "1"
            if (i < exchangeOneTopTexts.Length && exchangeOneTopTexts[i] != null)
                exchangeOneTopTexts[i].text = $"{Config.coinRate} {prevCoinName}";

            if (i < exchangeOneBotTexts.Length && exchangeOneBotTexts[i] != null)
            {
                BigInteger costOne = (currentTier == 0) ? Config.CoinExchangeCost * 5 : Config.CoinExchangeCost * BigInteger.Pow(currentTier, 5) * 5;
                exchangeOneBotTexts[i].text = $"{Formatter.FormatValue(costOne)} руб.";
            }

            // Кнопка "Макс"
            BigInteger possibleByCoins = GetCoinCountByIndex(prevTier) / Config.coinRate;
            BigInteger unitCost = (currentTier == 0) ? Config.CoinExchangeCost * 5 : Config.CoinExchangeCost * BigInteger.Pow(currentTier, 5) * 5;
            BigInteger possibleByMoney = unitCost > 0 ? Config.RubCount / unitCost : possibleByCoins;
            BigInteger maxPossible = (possibleByCoins < possibleByMoney) ? possibleByCoins : possibleByMoney;

            if (i < exchangeMaxTopTexts.Length && exchangeMaxTopTexts[i] != null)
                exchangeMaxTopTexts[i].text = $"{Formatter.FormatValue(maxPossible * Config.coinRate)} {prevCoinName}";

            if (i < exchangeMaxBotTexts.Length && exchangeMaxBotTexts[i] != null)
                exchangeMaxBotTexts[i].text = $"{Formatter.FormatValue(maxPossible * unitCost)} руб.";
        }

        if (exchangeHeaderDeltaText != null)
            exchangeHeaderDeltaText.text = $"Общий доход: +{Formatter.FormatValue(totalIncome)} руб / {interval} сек";
    }

    private BigInteger GetCoinCountByIndex(int index)
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

    private BigInteger GetCoinIncomeByIndex(int index)
    {
        return index switch
        {
            0 => 0,
            1 => Config.coinOlovoIncome,
            2 => Config.coinBronzaIncome,
            3 => Config.coinIronIncome,
            4 => Config.coinSilverIncome,
            5 => Config.coinGoldIncome,
            6 => Config.coinTitanIncome,
            7 => Config.coinDiamondIncome,
            8 => Config.coinObsidianIncome,
            9 => Config.coinRybinIncome,
            10 => Config.coinSingularIncome,
            _ => 0
        };
    }

    private string GetCoinNameByIndex(int index)
    {
        string[] names = { "Медная монета", "Оловянная монета", "Бронзовая монета", "Железная монета", "Серебряная монета", "Золотая монета", "Титановая монета", "Алмазная монета", "Обсидиановая монета", "Рубиновая монета", "Сингулярность" };
        return (index >= 0 && index < names.Length) ? names[index] : "Монета";
    }

    private string GetCoinShortNameByIndex(int index)
    {
        string[] shortNames = { "мед. мон", "ол. мон", "брон. мон", "жел. мон", "сер. мон",
            "зол. мон", "тит. мон", "алм. мон", "обс. мон", "руб. мон", "синг" };
        return (index >= 0 && index < shortNames.Length) ? shortNames[index] : "шт.";
    }

    private void HandleClickPowerUpdates()
    {
        if (tempPowerCopper != Config.ClickCopperPower) { tempPowerCopper = Config.ClickCopperPower; copperButtonText.text = $"+{Formatter.FormatValue(Config.ClickCopperPower)} Медь"; }
        if (tempPowerCoin != Config.ClickCoinPower) { tempPowerCoin = Config.ClickCoinPower; smelterButtonText.text = $"+{Formatter.FormatValue(Config.ClickCoinPower)} Монет"; }
        if (tempPowerRub != Config.ClickRubPower) { tempPowerRub = Config.ClickRubPower; coinButtonText.text = $"+{Formatter.FormatValue(Config.ClickRubPower)} Рублей"; }
    }

    private void UpdateStaticTexts()
    {
        tempPowerCopper = Config.ClickCopperPower; tempPowerCoin = Config.ClickCoinPower; tempPowerRub = Config.ClickRubPower;
        copperButtonText.text = $"+{Formatter.FormatValue(Config.ClickCopperPower * Config.GlobalMultiplier)} Медь";
        smelterButtonText.text = $"+{Formatter.FormatValue(Config.ClickCoinPower * Config.GlobalMultiplier)} Монет";
        coinButtonText.text = $"+{Formatter.FormatValue(Config.ClickRubPower * Config.GlobalMultiplier)} Рублей";
    }
}
