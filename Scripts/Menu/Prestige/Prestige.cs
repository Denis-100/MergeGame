using UnityEngine;

public class Prestige : MonoBehaviour
{
    public void buyPrestige()
    {
        if (Config.RubCount >= Config.presigePrice)
        {
            Config.RubCount -= Config.presigePrice;
            Config.presigePrice *= Config.prestigeMultiPrice;

            Config.prestigeCount++;
            Config.prestigePoints += Config.prestigeAddPoints;

            Config.GlobalMultiplier += Config.prestigeAddMulti;
        }
    }
}
