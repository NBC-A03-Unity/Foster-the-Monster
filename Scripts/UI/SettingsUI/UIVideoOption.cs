using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Enums;
public class UIVideoOption : MonoBehaviour
{
    private const int VSyncOn = 1;
    private const int VSyncOff = 0;

    private static readonly List<int> FrameRates = new List<int> { 240, 144, 120, 60, 30 };

    public Toggle fullScreenToggle;
    public Toggle vSyncToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown frameRateDropdown;
    public Button applyBtn;

    private FullScreenMode screenMode;
    private List<Resolution> resolutions = new List<Resolution>();
    private int resolutionNum;
    private int frameRateNum;
    private int resolutionOptionNum = 0;
    private int frameRateOptionNum = 0;

    private void Start()
    {
        SetAddListener();
        InitUI();
    }

    private void SetAddListener()
    {
        applyBtn.onClick.AddListener(OnApplyClicked);
    }

    private void InitUI()
    {
        InitResolutionDropdown();
        InitFrameRateDropdown();

        fullScreenToggle.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
    }

    private void InitResolutionDropdown()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutions.Reverse();
        resolutionDropdown.options.Clear();

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData
            {
                text = item.width + "x" + item.height
            };
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = resolutionOptionNum;
            resolutionOptionNum++;
        }
        resolutionDropdown.RefreshShownValue();
    }

    private void InitFrameRateDropdown()
    {
        frameRateDropdown.options.Clear();
        foreach (int frameRate in FrameRates)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData
            {
                text = frameRate.ToString()
            };
            frameRateDropdown.options.Add(option);

            if (frameRate == Application.targetFrameRate)
                frameRateDropdown.value = frameRateOptionNum;
            frameRateOptionNum++;
        }
        frameRateDropdown.RefreshShownValue();
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FrameRateOptionChange(int x)
    {
        frameRateNum = x;
    }

    public void FullScreenToggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void VSyncToggle(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? VSyncOn : VSyncOff;
    }

    public void OnApplyClicked()
    {
        UIManager.Instance.CloseUI<UISettings>();
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Check);
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
        Application.targetFrameRate = FrameRates[frameRateNum];
    }
}
