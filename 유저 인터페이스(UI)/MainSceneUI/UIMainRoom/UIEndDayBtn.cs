using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIEndDayBtn : MonoBehaviour
{
    private const string mainColor = "98F7C6";
    [SerializeField] private Button endDayBtn;
    [SerializeField] private TMP_Text endDayBtnHoverText;
    UID_DayTextBG UIDay;

    private void Awake()
    {
        endDayBtn.onClick.AddListener(OnEndDayBtn);
        UIManager.Instance.InitializeButtonHoverEffect(endDayBtn, endDayBtnHoverText, mainColor);
    }

    public void Init(UID_DayTextBG UIDay)
    {
        this.UIDay = UIDay;
    }

    public void OnEndDayBtn()
    {
        if (!DataManager.Instance.MainSceneTutorial) return;
        if (DataManager.Instance.selectCount > 0)
        {
            PopUpRemainCardSelect();
            return;
        }

        if (DataManager.Instance.resultCount != 0)
        {
            PopUpRemainResult();
            return;
        }

        if (!DataManager.Instance.todayVisitPlanet)
        {
            CheckPopUp(2008, 1008);
        }
        else
        {
            CheckPopUp(2004, 1004);
        }
    }
    public void PopUpRemainCardSelect()
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? 4011 : 3011;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
            }
        );
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
    public void CheckPopUp(int e, int k)
    {
        int exitPopupKey = GlobalSettings.CurrentLocale == "en-US" ? e : k;

        UIManager.Instance.OpenConfirmationPopup(
            exitPopupKey,
            () =>
            {
                UIManager.Instance.CloseUI<ConfirmationPopup>();
                CompleteEndDay();
            },
            () =>
            {
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );
    }
    public void CompleteEndDay()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.EndDay);
        DataManager.Instance.date++;
        DataManager.Instance.todayVisitPlanet = false;
        CageManager.Instance.EndDay();
        UIDay.UpdateDayTxt();
        UIManager.Instance.OpenUI<UIChangeDay>();
    }
}
