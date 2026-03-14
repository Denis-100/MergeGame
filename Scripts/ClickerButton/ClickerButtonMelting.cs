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

        // 1. Получаем базовую силу клика из формул
        BigInteger clickPower = GameFormulas.GetTotalClickCoin();

        // 2. Считаем, сколько меди нужно для ПОЛНОГО клика
        BigInteger totalCost = clickPower * Config.MeltingCost;

        // 3. Проверяем ресурсы
        if (Config.CopperCount >= totalCost)
        {
            // Хватает на полный клик
            Config.CopperCoinCount += clickPower;
            Config.CopperCount -= totalCost;
            SaveManager.SaveGame();
        }
        else if (Config.CopperCount >= Config.MeltingCost)
        {
            // Хватает только на часть клика (плавим сколько можем)
            BigInteger possibleAmount = Config.CopperCount / Config.MeltingCost;

            Config.CopperCoinCount += possibleAmount;
            Config.CopperCount -= possibleAmount * Config.MeltingCost;
            SaveManager.SaveGame();
        }
    }
}
