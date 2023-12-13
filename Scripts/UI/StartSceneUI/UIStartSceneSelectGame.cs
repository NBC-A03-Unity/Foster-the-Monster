using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIStartSceneSelectGame : MonoBehaviour
{
    [SerializeField] private Button newGameStartBtn;
    [SerializeField] private Button loadGameStartBtn;
    [SerializeField] private Button returnBtn;

    private ColorChanger colorChanger;

    private void Start()
    {
        colorChanger = GetComponent<ColorChanger>() ?? gameObject.AddComponent<ColorChanger>();
        InitializeButtonListeners();
        InitializeButtonHoverEffects();
    }

    private void InitializeButtonListeners()
    {
        newGameStartBtn.onClick.AddListener(OnNewGameStartBtn);
        loadGameStartBtn.onClick.AddListener(OnLoadGameStartBtn);
        returnBtn.onClick.AddListener(OnReturnClicked);
    }

    private void OnNewGameStartBtn()
    {
        int popupKey = GlobalSettings.CurrentLocale == "en-US" ? 2002 : 1002;
        UIManager.Instance.OpenConfirmationPopup(
            popupKey,
            () => {
                StartNewGame();
            },
            () => {
                AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );
    }

    private void OnLoadGameStartBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UISaveLoad>();
    }

    private void InitializeButtonHoverEffects()
    {
        foreach (Button button in new Button[] { newGameStartBtn, loadGameStartBtn, returnBtn })
        {
            colorChanger.AddHoverEffect(button.GetComponent<Image>());
        }
    }

    private void OnReturnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        UIManager.Instance.CloseUI<UIStartSceneSelectGame>();
    }

    private void StartNewGame()
    {
        AudioManager.Instance.StopBGM();
        DataManager.Instance.ResetDataForNewGame();
        SaveLoadManager.Instance.SaveData(DataManager.Instance);
        LoadingSceneController.LoadScene(SceneName.MovieScene);
    }
}
