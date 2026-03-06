using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // Универсальный метод: передаем клип прямо из инспектора кнопки
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // Используем громкость из Config
            audioSource.PlayOneShot(clip, Config.MasterVolume);
        }
    }
}