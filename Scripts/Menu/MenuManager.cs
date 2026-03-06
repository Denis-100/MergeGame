using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Список всех панелей меню")]
    public GameObject[] panels;

    // Метод для открытия конктреной панели по индексу
    public void OpenPanel(int panelIndex)
    {
        // Сначала выключение всех панелей
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        // Включение нужной панели при верном индексе
        if(panelIndex >= 0 && panelIndex <= panels.Length)
        {
            panels[panelIndex].SetActive(true);
        }
    }

    // Вернуться на главный экран
    public void BackToMain()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
