using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [Header("Элементы UI")]
    public Slider volumeSlider;       
    public TextMeshProUGUI volumeText;

    [Header("Элементы UI (Музыка)")]
    public Slider musicSlider;
    public TextMeshProUGUI musicText;

    [Header("Для звука")]
    public SoundManager soundManager;
    public AudioClip previewClip;
    public AudioSource musicSource;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // При старте устанавливаем значение ползунка из сохраненного конфига
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = Config.MasterVolume;

            // Подписываемся на изменение значения ползунка кодом
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.value = Config.MusicVolume;
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        UpdateVolumeText(Config.MasterVolume);
        UpdateMusicText(Config.MusicVolume);

        // Запускаем музыку сразу при старте, если есть источник
        if (musicSource != null)
        {
            musicSource.volume = Config.MusicVolume;
            musicSource.loop = true;
            if (!musicSource.isPlaying) musicSource.Play();
        }
    }

    // Метод вызывается при каждом движении ползунка
    public void OnVolumeChanged(float value)
    {
        Config.MasterVolume = value;
        UpdateVolumeText(value);

        // Опционально: сохраняем сразу, чтобы при перезагрузке звук остался прежним
        PlayerPrefs.SetFloat("MasterVolume", value);
        SaveManager.SaveGame();
    }

    public void PlayPreview()
    {
        if (soundManager != null && previewClip != null)
        {
            soundManager.PlaySound(previewClip);
        }
    }

    private void UpdateVolumeText(float value)
    {
        if (volumeText != null)
        {
            // Переводим 0..1 в 0..100%
            int percent = Mathf.RoundToInt(value * 100);
            volumeText.text = $"Громкость: {percent}%";
        }
    }

    // --- Музыка ---
    // Метод для музыки
    public void OnMusicChanged(float value)
    {
        Config.MusicVolume = value;
        UpdateMusicText(value);
        PlayerPrefs.SetFloat("MusicVolume", value);

        if (musicSource != null)
        {
            musicSource.volume = value;
        }
        SaveManager.SaveGame();
    }

    private void UpdateMusicText(float value)
    {
        if (musicText != null)
        {
            int percent = Mathf.RoundToInt(value * 100);
            musicText.text = $"Музыка: {percent}%";
        }
    }
}
