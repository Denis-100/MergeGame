using UnityEngine;
using TMPro;

public class LogItemUI : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;
    public GameObject bodyObject;

    public void Setup(ChangelogData data)
    {
        headerText.text = $"Версия {data.version} ({data.date})";
        bodyText.text = data.changes;
        bodyObject.SetActive(false); // По умолчанию закрыто
    }

    public void ToggleDescription()
    {
        bodyObject.SetActive(!bodyObject.activeSelf);

        Canvas.ForceUpdateCanvases();
    }
}