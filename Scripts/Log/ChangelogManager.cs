using UnityEngine;
using System.Collections.Generic;

public class ChangelogManager : MonoBehaviour
{
    public GameObject logPanel;
    public GameObject logItemPrefab;
    public Transform container;
    public List<ChangelogData> allLogs; // Сюда перетащите ваши Scriptable Objects
    public string lastVersionKey = "LastSeenVersion";


    void Start()
    {
        CheckForUpdate();
    }

    void CheckForUpdate()
    {
        string currentVersion = Application.version;
        string lastVersion = PlayerPrefs.GetString(lastVersionKey, "0.0");


        if (currentVersion != lastVersion)
        {
            BuildList();
            logPanel.SetActive(true); // Показываем окно
            PlayerPrefs.SetString(lastVersionKey, currentVersion);
        }
        else
        {
            logPanel.SetActive(false); // Версия та же — скрываем
        }
    }

    public void OpenChangelog()
    {
        BuildList();
        logPanel.SetActive(true);
    }

    public void CloseChangelog()
    {
        logPanel.SetActive(false);
    }

    void BuildList()
    {
        // Очистка старых элементов
        foreach (Transform child in container) Destroy(child.gameObject);

        // Создание новых (идем с конца списка, чтобы новые были сверху)
        for (int i = allLogs.Count - 1; i >= 0; i--)
        {
            GameObject go = Instantiate(logItemPrefab, container);
            go.GetComponent<LogItemUI>().Setup(allLogs[i]);
        }
    }
}