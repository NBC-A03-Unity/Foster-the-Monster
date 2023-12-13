using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
public class UIPlanetBtnBG : MonoBehaviour
{
    private const string mainColor = "98F7C6";
    [SerializeField] private Button planetBtn;
    [SerializeField] private TMP_Text planetBtnHoverText;

    private void Awake()
    {
        planetBtn.onClick.AddListener(OnPlanetBtn);
        UIManager.Instance.InitializeButtonHoverEffect(planetBtn, planetBtnHoverText, mainColor);
    }
    public void OnPlanetBtn()
    {
        if (!DataManager.Instance.MainSceneTutorial) return;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);

        if (DataManager.Instance.resultCount != 0)
        {
            PopUpRemainResult();
        }
        else if (DataManager.Instance.todayVisitPlanet)
        {
            PopUpWaringPlant();
        }
        else
        {
            DataManager.Instance.todayVisitPlanet = true;
            UIManager.Instance.OpenUI<UIPlanetChlorophyllis>();
            UIManager.Instance.CloseUI<UIMainRoom>();
        }

    }

    public void PopUpRemainResult()
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? 4000 : 3000;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
            }
        );
    }

    public void PopUpWaringPlant()
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? 4003 : 3003;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
            }
        );
    }
}
