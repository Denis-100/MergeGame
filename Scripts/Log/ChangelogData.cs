using UnityEngine;

[CreateAssetMenu(fileName = "NewVersion", menuName = "Changelog/VersionData")]
public class ChangelogData : ScriptableObject
{
    public string version;
    public string date;
    [TextArea(5, 10)] // ѕозвол€ет удобно писать текст в инспекторе
    public string changes;
}