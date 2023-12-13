using UnityEngine;
using UnityEngine.UI;

public class UIAudioOption : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider BGMvolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        BGMvolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        masterVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.MasterSoundVolume);
        BGMvolumeSlider.onValueChanged.AddListener(AudioManager.Instance.BGMSoundVolume);
        SFXVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SFXSoundVolume);
    }
}
