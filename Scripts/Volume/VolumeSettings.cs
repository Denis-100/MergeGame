using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [Header("Элементы UI")]
    public Slider volumeSlider;       
    public TextMeshProUGUI volumeText;

    [Header("Для звука")]
    public SoundManager soundManager;
    public AudioClip previewClip;

    private void Start()
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

        UpdateVolumeText(Config.MasterVolume);
    }

    // Метод вызывается при каждом движении ползунка
    public void OnVolumeChanged(float value)
    {
        Config.MasterVolume = value;
        UpdateVolumeText(value);

        // Опционально: сохраняем сразу, чтобы при перезагрузке звук остался прежним
        PlayerPrefs.SetFloat("MasterVolume", value);
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
}
