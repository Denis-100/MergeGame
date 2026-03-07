using UnityEngine;
using System.Numerics;
using System;

public class UpgradesManager : MonoBehaviour
{
    private void Save() => SaveManager.SaveGame();

    // --- ЛОГИКА ШАХТ ---
    public void BuyMine()
    {
        if (Config.RubCount >= Config.MineBuyCost)
        {
            Config.RubCount -= Config.MineBuyCost;
            Config.MineCount++;
            Config.MineBuyCost = (Config.MineBuyCost * (BigInteger)(Config.MultiMineBuy * 100)) / 100;
            Save();
        }
    }

    public void UpgradeMinePower()
    {
        if (Config.RubCount >= Config.MinePowerCost)
        {
            Config.RubCount -= Config.MinePowerCost;
            Config.MinePower += Config.MineAddPower;
            Config.MinePowerCost = (Config.MinePowerCost * (BigInteger)(Config.MultiMinePower * 100)) / 100;
            Save();
        }
    }

    public void UpgradeMineSpeed()
    {
        if (Config.RubCount >= Config.MineSpeedCost && Config.MineTimer > Config.MineMinTimer)
        {
            Config.RubCount -= Config.MineSpeedCost;
            Config.MineTimer -= 0.1f;
            Config.MineSpeedCost = (Config.MineSpeedCost * (BigInteger)(Config.MultiMineSpeed * 100)) / 100;
            Save();
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
            Save();
        }
    }

    public void UpgradeSmelterPower()
    {
        if (Config.RubCount >= Config.SmelterPowerCost)
        {
            Config.RubCount -= Config.SmelterPowerCost;
            Config.SmelterPower++;
            Config.SmelterPowerCost = (Config.SmelterPowerCost * (BigInteger)(Config.MultiSmelterPower * 100)) / 100;
            Save();
        }
    }

    public void UpgradeSmelterSpeed()
    {
        if (Config.RubCount >= Config.SmelterSpeedCost && Config.SmelterTimer > Config.SmelterMinTimer)
        {
            Config.RubCount -= Config.SmelterSpeedCost;
            Config.SmelterTimer -= 0.1f;
            Config.SmelterSpeedCost = (Config.SmelterSpeedCost * (BigInteger)(Config.MultiSmelterSpeed * 100)) / 100;
            Save();
        }
    }

    // --- ЛОГИКА УЛУЧШЕНИЯ КЛИКОВ (УНИВЕРСАЛЬНАЯ) ---

    public void UpgradeClickCopper() => UpgradeClickGeneric(ref Config.lvlClickCopper, ref Config.ClickCopperPower,
        ref Config.CostClickCopper, Config.growthRateCopper, Config.MultiClickCopperPower, Config.MultiClickCopper);
    public void UpgradeClickCoin() => UpgradeClickGeneric(ref Config.lvlClickCoin, ref Config.ClickCoinPower,
        ref Config.CostClickCoin, Config.growthRateCoin, Config.MultiClickCoinPower, Config.MultiClickCoin);
    public void UpgradeClickRub() => UpgradeClickGeneric(ref Config.lvlClickRub, ref Config.ClickRubPower,
        ref Config.CostClickRub, Config.growthRateRub, Config.MultiClickRubPower, Config.MultiClickRub);

    // Сама логика прокачки (ref позволяет менять данные прямо в Config)
    private void UpgradeClickGeneric(ref int lvl, ref BigInteger power,
        ref BigInteger cost, float growthRate, float pwrMulti, float costMulti)
    {
        // 1. Проверяем, хватает ли рублей на покупку
        if (Config.RubCount >= cost)
        {
            // 2. Списываем деньги и повышаем уровень
            Config.RubCount -= cost;
            lvl++;

            // 3. Расчитываем ПРИБАВКУ к силе клика
            // Используем Math.Pow для экспоненциального роста (чем выше LVL, тем больше прибавка)
            BigInteger expGrowth = (BigInteger)Math.Pow(growthRate, lvl);
            BigInteger addPower = 1 + expGrowth;

            // Применяем множитель силы (через *100 /100, так как BigInteger не любит дроби)
            addPower = (addPower * (BigInteger)(pwrMulti * 100)) / 100;

            // 4. Обновляем текущую силу клика
            power += addPower;

            // 5. Увеличиваем СТОИМОСТЬ следующего улучшения
            cost = (cost * (BigInteger)(costMulti * 100)) / 100;

            // 6. Сохраняем прогресс
            Save();
        }
    }
}