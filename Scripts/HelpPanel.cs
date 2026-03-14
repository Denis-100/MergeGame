using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    public GameObject helpPanel;

    void Start()
    {
        if (Config.IsFirstRun == 0)
        {
            ShowHelpPanel();
            Config.IsFirstRun = 1;
            SaveManager.SaveGame();
        }
    }

    private void ShowHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
    }
}
