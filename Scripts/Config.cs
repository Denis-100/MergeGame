using UnityEditor;
using System.Numerics;
using Unity.Burst.CompilerServices;

public static class Config
{
    // Информация о балансе
    public static BigInteger CopperCount = 0;      // Количество медной руды
    public static BigInteger CopperCoinCount = 0;  // Количество медных монет
    public static BigInteger RubCount = 0;        // Количество рублей


    // ----- Шахты -----

    public static int MineCount = 0;               // Купленных шахт
    public static BigInteger MinePower = 3;        // Сколько руды дает за цикл
    public const int MineAddPower = 3;             // Сколько силы прибавляется за лвл ап
    public static float MineTimer = 7.0f;          // За сколько добывает
    public const float MineMinTimer = 0.1f;        // Лимит скорости

    // Цены шахт
    public static BigInteger MineBuyCost = 150;     // Цена шахты
    public static BigInteger MinePowerCost = 300;  // Цена улучшения сколько добывает за цикл
    public static BigInteger MineSpeedCost = 500;  // Цена улучшения скорости

    // Множитель улучшений шахт
    public static float MultiMineBuy = 1.2f;       // Рост цены покупки
    public static float MultiMinePower = 1.2f;     // Рост цены силы
    public static float MultiMineSpeed = 1.4f;     // Рост цены скорости


    // ----- Плавильни -----

    public static int SmelterCount = 0;            // Купленных плавилен
    public static BigInteger SmelterPower = 1;     // Сколько плавит за раз
    public static float SmelterTimer = 7.0f;       // За сколько плавит
    public const float SmelterMinTimer = 0.1f;     // Лимит скорости
    public const int MeltingCost = 3;              // Цена 1 монеты в руде

    // Цены плавилен 
    public static BigInteger SmelterBuyCost = 150;     // Цена плавильни
    public static BigInteger SmelterPowerCost = 300;   // Цена улучшение сколько плавит за цикл
    public static BigInteger SmelterSpeedCost = 500;   // Цена улучшения скорости

    //Множитель улучшений плавилен
    public static float MultiSmelterBuy = 1.2f;    // Рост цены покупки
    public static float MultiSmelterPower = 1.2f;  // Рост цены силы
    public static float MultiSmelterSpeed = 1.4f;  // Рост цены скорости

    // Клик
    public static int lvlClickCopper = 1;             // Ур улучшения клика руды
    public static BigInteger ClickCopperPower = 1;    // Сила клика руды
    public static BigInteger CostClickCopper = 20;    // Цена апа клика руды
    public static float MultiClickCopper = 1.2f;      // Множитель цены клика руды
    public static float MultiClickCopperPower = 1.4f; // Множитель силы клика руды
    public const float growthRateCopper = 1.09f;      // Множитель прибавки к силе клике за ур

    public static int lvlClickCoin = 1;               // Ур улучшения клика переплавки
    public static BigInteger ClickCoinPower = 1;      // Сила клика монет (сколько плавим за раз)
    public static BigInteger CostClickCoin = 20;      // Цена апа клика монет
    public static float MultiClickCoin = 1.2f;        // Множитель цены клика монет
    public static float MultiClickCoinPower = 1.1f;   // Множитель силы клика монет
    public const float growthRateCoin = 1.08f;        // Множитель прибавки к силе клике за ур

    public static int lvlClickRub = 1;                // Ур улучшения клика рублей
    public static BigInteger ClickRubPower = 1;       // Сила клика рублей
    public static BigInteger CostClickRub = 20;       // Цена апа клика рублей
    public static float MultiClickRub = 1.2f;         // Множитель цены клика рублей 
    public static float MultiClickRubPower = 1.2f;    // Множитель силы клика рублей 
    public const float growthRateRub = 1.09f;         // Множитель прибавки к силе клике за ур

    // Перерождение
    public static BigInteger presigePrice = 100_000_000;     // Цена 1-го престижа
    public const int prestigeMultiPrice = 100;        // Множитель цены престижа
    public static int prestigeCount = 0;              // Количество перерождение
    public static BigInteger prestigePoints = 0;      // Количество очков перерождений
    public static BigInteger prestigeAddPoints = 1;      // Количество очков перерождений
    public static int prestigeAddMulti = 1;      // Бонус от перерождения к множителю      
    public static int GlobalMultiplier = 1;      // Множитель от перерождений

    // ----- Монеты -----
    // Количество
    public static BigInteger coinOlovoCount = 0;     // Олово
    public static BigInteger coinBronzaCount = 0;    // Бронза
    public static BigInteger coinIronCount = 0;      // Железо
    public static BigInteger coinSilverCount = 0;    // Серебро
    public static BigInteger coinGoldCount = 0;      // Золото
    public static BigInteger coinTitanCount = 0;     // Титан
    public static BigInteger coinDiamondCount = 0;   // Алмаз
    public static BigInteger coinObsidianCount = 0;  // Обсидиан
    public static BigInteger coinRybinCount = 0;     // Рубин
    public static BigInteger coinSingularCount = 0;  // Сингулярность

    // Курс обмена монет
    public const int coinRate = 10;                  // Количество монет необходимых для след уровня монеты
    public static BigInteger CoinExchangeCost = 10;   // Стоимость одной операции обмена в рублях

    // Базовый доход монет
    public static BigInteger coinOlovoIncome = 1;         // Олово
    public static BigInteger coinBronzaIncome = 15;        // Бронза
    public static BigInteger coinIronIncome = 300;         // Железо
    public static BigInteger coinSilverIncome = 5_000;       // Серебро
    public static BigInteger coinGoldIncome = 70_000;        // Золото
    public static BigInteger coinTitanIncome = 1_000_000;     // Титан
    public static BigInteger coinDiamondIncome = 15_000_000;  // Алмаз
    public static BigInteger coinObsidianIncome = 200_000_000; // Обсидиан
    public static BigInteger coinRybinIncome = 2_500_000_000;           // Рубин
    public static BigInteger coinSingularIncome = 50_000_000_000;        // Сингулярность

    // Громкость звука
    public static float MasterVolume = 0.5f;

    public static void ResetData()
    {
        // Информация о балансе
        CopperCount = 0;        // Количество медной руды
        CopperCoinCount = 0;    // Количество медных монет
        RubCount = 0;          // Количество рублей
        //CopperCount = 1000000000000000;        // Количество медной руды
        //CopperCoinCount = 1000000000000000;    // Количество медных монет
        //RubCount = 1000000000000000;          // Количество рублей

        // ----- Шахты -----
        MineCount = 0;        // Купленных шахт
        MinePower = 3;        // Сколько руды дает за цикл
        MineTimer = 7.0f;     // За сколько добывает

        // Цены шахт
        MineBuyCost = 150;     // Цена шахты
        MinePowerCost = 300;  // Цена улучшения сколько добывает за цикл
        MineSpeedCost = 500;  // Цена улучшения скорости

        // Множитель улучшений шахт
        MultiMineBuy = 1.2f;     // Рост цены покупки
        MultiMinePower = 1.2f;   // Рост цены силы
        MultiMineSpeed = 1.4f;   // Рост цены скорости

        // ----- Плавильни -----
        SmelterCount = 0;         // Купленных плавилен
        SmelterPower = 1;         // Сколько плавит за раз
        SmelterTimer = 7.0f;      // За сколько плавит

        // Цены плавилен 
        SmelterBuyCost = 150;     // Цена плавильни
        SmelterPowerCost = 300;   // Цена улучшение сколько плавит за цикл
        SmelterSpeedCost = 500;   // Цена улучшения скорости

        // Множитель улучшений плавилен
        MultiSmelterBuy = 1.2f;    // Рост цены покупки
        MultiSmelterPower = 1.2f;  // Рост цены силы
        MultiSmelterSpeed = 1.4f;  // Рост цены скорости

        // Клик
        lvlClickCopper = 1;           // Ур улучшения клика меди
        ClickCopperPower = 1;         // Сила клика руды
        CostClickCopper = 20;         // Цена апа клика руды
        MultiClickCopper = 1.2f;      // Множитель цены клика руды
        MultiClickCopperPower = 1.3f; // Множитель силы клика руды

        lvlClickCoin = 1;           // Ур улучшения клика переплавки
        ClickCoinPower = 1;         // Сила клика монет (сколько плавим за раз)
        CostClickCoin = 20;         // Цена апа клика монет
        MultiClickCoin = 1.2f;      // Множитель цены клика монет
        MultiClickCoinPower = 1.3f; // Множитель силы клика монет

        lvlClickRub = 1;           // Ур улучшения клика рублей
        ClickRubPower = 1;         // Сила клика рублей
        CostClickRub = 20;        // Цена апа клика рублей
        MultiClickRub = 1.2f;      // Множитель цены клика рублей 
        MultiClickRubPower = 1.3f; // Множитель силы клика рублей 

        // Перерождение
        presigePrice = 100_000_000;     // Цена 1-го престижа
        prestigeCount = 0;         // Количество перерождение
        prestigePoints = 0;        // Количество очков перерождений
        GlobalMultiplier = 1;   // Множитель от перерождений

        // ----- Монеты -----
        coinOlovoCount = 0;     // Олово
        coinBronzaCount = 0;    // Бронза
        coinIronCount = 0;      // Железо
        coinSilverCount = 0;    // Серебро
        coinGoldCount = 0;      // Золото
        coinTitanCount = 0;     // Титан
        coinDiamondCount = 0;   // Алмаз
        coinObsidianCount = 0;  // Обсидиан
        coinRybinCount = 0;     // Рубин
        coinSingularCount = 0;  // Сингулярность

        // Громкость звука
        MasterVolume = 0.5f;


}
}
