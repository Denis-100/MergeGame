using UnityEngine;
using System.Numerics;

public class UpgradesManager : MonoBehaviour
{
    private void Save() => SaveManager.SaveGame();

    public void BuyMine()
    {
        if (Config.RubCount >= Config.MineBuyCost)
        {
            Config.RubCount -= Config.MineBuyCost;
            Config.MineCount++;
            Config.MineBuyCost = GameFormulas.GetNextCost(Config.MineBuyCost, Config.MultiMineBuy);
            Save();
        }
    }

    public void UpgradeMinePower()
    {
        if (Config.RubCount >= Config.MinePowerCost)
        {
            Config.RubCount -= Config.MinePowerCost;
            Config.MinePower += Config.MineAddPower;
            Config.MinePowerCost = GameFormulas.GetNextCost(Config.MinePowerCost, Config.MultiMinePower);
            Save();
        }
    }

    public void UpgradeMineSpeed()
    {
        if (Config.RubCount >= Config.MineSpeedCost && Config.MineTimer > Config.MineMinTimer)
        {
            Config.RubCount -= Config.MineSpeedCost;
            Config.MineTimer -= 0.1f;
            Config.MineSpeedCost = GameFormulas.GetNextCost(Config.MineSpeedCost, Config.MultiMineSpeed);
            Save();
        }
    }

    public void BuySmelter()
    {
        if (Config.RubCount >= Config.SmelterBuyCost)
        {
            Config.RubCount -= Config.SmelterBuyCost;
            Config.SmelterCount++;
            Config.SmelterBuyCost = GameFormulas.GetNextCost(Config.SmelterBuyCost, Config.MultiSmelterBuy);
            Save();
        }
    }

    public void UpgradeSmelterPower()
    {
        if (Config.RubCount >= Config.SmelterPowerCost)
        {
            Config.RubCount -= Config.SmelterPowerCost;
            Config.SmelterPower++;
            Config.SmelterPowerCost = GameFormulas.GetNextCost(Config.SmelterPowerCost, Config.MultiSmelterPower);
            Save();
        }
    }

    public void UpgradeSmelterSpeed()
    {
        if (Config.RubCount >= Config.SmelterSpeedCost && Config.SmelterTimer > Config.SmelterMinTimer)
        {
            Config.RubCount -= Config.SmelterSpeedCost;
            Config.SmelterTimer -= 0.1f;
            Config.SmelterSpeedCost = GameFormulas.GetNextCost(Config.SmelterSpeedCost, Config.MultiSmelterSpeed);
            Save();
        }
    }

    public void UpgradeClickCopper() => UpgradeClickGeneric(ref Config.lvlClickCopper, ref Config.ClickCopperPower, ref Config.CostClickCopper, Config.growthRateCopper, Config.MultiClickCopperPower, Config.MultiClickCopper);
    public void UpgradeClickCoin() => UpgradeClickGeneric(ref Config.lvlClickCoin, ref Config.ClickCoinPower, ref Config.CostClickCoin, Config.growthRateCoin, Config.MultiClickCoinPower, Config.MultiClickCoin);
    public void UpgradeClickRub() => UpgradeClickGeneric(ref Config.lvlClickRub, ref Config.ClickRubPower, ref Config.CostClickRub, Config.growthRateRub, Config.MultiClickRubPower, Config.MultiClickRub);

    private void UpgradeClickGeneric(ref int lvl, ref BigInteger power, ref BigInteger cost, float growthRate, float pwrMulti, float costMulti)
    {
        if (Config.RubCount >= cost)
        {
            Config.RubCount -= cost;
            lvl++;
            power += GameFormulas.CalculateClickUpgradePower(lvl, growthRate, pwrMulti);
            cost = GameFormulas.GetNextCost(cost, costMulti);
            Save();
        }
    }
}