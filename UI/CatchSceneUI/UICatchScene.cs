using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UICatchScene : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button endofDayBtn;
    [SerializeField] private Button helpBtn;
    private void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        pauseBtn.onClick.AddListener(OnPauseBtn);
        endofDayBtn.onClick.AddListener(OnEndofDayBtn);
        helpBtn.onClick.AddListener(OnHelpBtn);
    }

    private void OnHelpBtn()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIHelp>();
    }

    public void OnEndofDayBtn()
    {
        Time.timeScale = 0;
        int exitPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2001 : 1001;

        UIManager.Instance.OpenConfirmationPopup(
            exitPopupKey,
            () => {
                UIManager.Instance.CloseUI<ConfirmationPopup>();
                UIManager.Instance.OpenUI<UIResult>();
            },
            () => {
                Time.timeScale = 1.0f;
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );
    }

    public void OnResultComplete()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        AudioManager.Instance.StopBGM();
        LoadingSceneController.LoadScene(SceneName.MainScene);
        Time.timeScale = 1.0f;
    }

    public void OnPauseBtn()
    {
        Time.timeScale = 0;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIPause>();
    }
}
