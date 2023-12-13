using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Button returnBtn;

    private void Start()
    {
        SetAddListener();
    }

    private void SetAddListener()
    {
        returnBtn.onClick.AddListener(OnReturnClicked);
    }

    private void OnReturnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        SaveAudioSettings();
        UIManager.Instance.CloseUI<UISettings>();
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", AudioManager.Instance.GetVolume("Master"));
        PlayerPrefs.SetFloat("BGMVolume", AudioManager.Instance.GetVolume("BGM"));
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.Instance.GetVolume("SFX"));
        PlayerPrefs.Save();
    }
}
