using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public GameObject confirmWindow;


    // Открывает окно с подтверждением о сбросе прогресса
    public void AskReset()
    {
        if (confirmWindow != null)
        {
            confirmWindow.SetActive(true);
        }
    }

    // Сброс данные если игрок выбрал "Да"
    public void ConfirmReset()
    {
        SaveManager.ResetGame();
        confirmWindow.SetActive(false);

        FindFirstObjectByType<MenuManager>().BackToMain();
    }

    // Если игрок выбрал "Нет"
    public void CancelReset()
    {
        if (confirmWindow != null)
        {
            confirmWindow.SetActive(false);
        }
    }
    
}
