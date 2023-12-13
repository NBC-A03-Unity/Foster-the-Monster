using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIPause : MonoBehaviour
{
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button loadBtn;
    [SerializeField] private Button exitBtn;
    

    private void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        settingBtn.onClick.AddListener(OnSettingBtn);
        closeBtn.onClick.AddListener(OnCloseBtn);
        loadBtn.onClick.AddListener(OnLoadBtn);
        exitBtn.onClick.AddListener(OnExitBtn);
    }

    public void OnSettingBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Pause);
        UIManager.Instance.OpenUI<UISettings>();
    }

    public void OnLoadBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UISaveLoad>();
    }

    public void OnCloseBtn()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        UIManager.Instance.CloseUI<UIPause>();
    }

    public void OnExitBtn()
    {
        int exitPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2000 : 1000;

        UIManager.Instance.OpenConfirmationPopup(
            exitPopupKey,
            () => {
                AudioManager.Instance.StopBGM();
                LoadingSceneController.LoadScene(SceneName.StartScene);
            },
            () => {
                AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );
    }
}
