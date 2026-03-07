using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickerButtonMelting : MonoBehaviour
{
    private UnityEngine.Vector2 lastClickPosition;
    private int strike = 0;
    private const int MAX_STRIKE = 7;

    public void ClickMelting()
    {
        UnityEngine.Vector2 currentClickPos = Pointer.current.position.ReadValue();

        if (currentClickPos == lastClickPosition)
        {
            strike++;
            if (strike >= MAX_STRIKE)
            {
                Debug.Log("Обнаружен автокликер");
                return;
            }
        }
        else strike = 0;
        lastClickPosition = currentClickPos;

        BigInteger totalCost = Config.MeltingCost * Config.ClickCoinPower * Config.GlobalMultiplier;

        if (Config.CopperCount >= totalCost)
        {
            Config.CopperCoinCount += Config.ClickCoinPower * Config.GlobalMultiplier;
            Config.CopperCount -= totalCost;

            SaveManager.SaveGame();
        }
        
        else if (Config.CopperCount >= Config.MeltingCost * Config.GlobalMultiplier)
        {
            BigInteger possibleAmount = Config.CopperCount / Config.MeltingCost * Config.GlobalMultiplier;
            Config.CopperCoinCount += possibleAmount;
            Config.CopperCount -= possibleAmount * Config.MeltingCost * Config.GlobalMultiplier;

            SaveManager.SaveGame();
        }
        
    }
}
