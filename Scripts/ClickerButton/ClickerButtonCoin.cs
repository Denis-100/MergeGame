using UnityEngine;
using UnityEngine.InputSystem;

public class ClickerButtonCoin : MonoBehaviour
{
    private Vector2 lastClickPosition;
    private int strike = 0;
    private const int MAX_STRIKE = 7;

    public void ClickCoin()
    {
        Vector2 currentClickPos = Pointer.current.position.ReadValue();

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

        Config.RubCount += Config.ClickRubPower;
        SaveManager.SaveGame();
    }
}
