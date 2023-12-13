using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIStartScene : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button gameStartBtn;
    [SerializeField] private Button fosmonDictionaryBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button gameExitBtn;

    private ColorChanger colorChanger;

    private void Awake()
    {
        ObjectManager.Instance.Clear();
    }
    private void Start()
    {
        colorChanger = GetComponent<ColorChanger>() ?? gameObject.AddComponent<ColorChanger>();

        InitializeButtonListeners();
        InitializeButtonHoverEffects();
    }

    private void InitializeButtonListeners()
    {
        gameStartBtn.onClick.AddListener(OnGameStartClicked);
        fosmonDictionaryBtn.onClick.AddListener(OnFosmonDictionaryClicked);
        settingBtn.onClick.AddListener(OnSettingClicked);
        gameExitBtn.onClick.AddListener(OnGameExitClicked);
    }

    private void InitializeButtonHoverEffects()
    {
        foreach (Button button in new Button[] { gameStartBtn, fosmonDictionaryBtn, settingBtn, gameExitBtn })
        {
            colorChanger.AddHoverEffect(button.GetComponent<Image>());
        }
    }

    private void OnGameStartClicked()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIStartSceneSelectGame>();
    }

    public void OnFosmonDictionaryClicked()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIPlanetDictionary>();
    }

    private void OnSettingClicked()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UISettings>();
    }

    private void OnGameExitClicked()
    {
        int skipPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2010 : 1010;
        UIManager.Instance.OpenConfirmationPopup(
            skipPopupKey,
            () => {
                AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
                Application.Quit();
            },
            () => {
                AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );
    }
}
