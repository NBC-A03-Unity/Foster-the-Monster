using UnityEngine;

public class UIMainRoom : MonoBehaviour
{   
    UID_DayTextBG UIDay;
    UIDeckBtn UIDeckBtn;
    UIEndDayBtn UIEndDayBtn;
    UIUpgradeBtnBG UIUpgradeBtn;
    UIPlanetBtnBG UIPlanetBtnBG;
    private void OnEnable()
    {
        UIDay = UIManager.Instance.OpenUI<UID_DayTextBG>();
        UIDeckBtn = UIManager.Instance.OpenUI<UIDeckBtn>();
        UIEndDayBtn = UIManager.Instance.OpenUI<UIEndDayBtn>();
        UIEndDayBtn.Init(UIDay);
        UIUpgradeBtn = UIManager.Instance.OpenUI<UIUpgradeBtnBG>();
        UIPlanetBtnBG = UIManager.Instance.OpenUI<UIPlanetBtnBG>();

        if (!DataManager.Instance.MainSceneTutorial)
        {
            UITutorial tutorial = UIManager.Instance.OpenUI<UITutorial>();
            tutorial.Canvass[1] = UIDay.GetComponent<Canvas>();
            tutorial.Canvass[2] = UIUpgradeBtn.GetComponent<Canvas>();
            tutorial.Canvass[3] = UIPlanetBtnBG.GetComponent<Canvas>();
            tutorial.Canvass[4] = UIDeckBtn.GetComponent<Canvas>();
            tutorial.Canvass[5] = UIEndDayBtn.GetComponent<Canvas>();
            tutorial.Canvass[6] = MainSceneGameManager.Instance.UICage.GetComponent<Canvas>();
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.CloseUI<UIPlanetBtnBG>();
        UIManager.Instance.CloseUI<UIUpgradeBtnBG>();
        UIManager.Instance.CloseUI<UIEndDayBtn>();
        UIManager.Instance.CloseUI<UIDeckBtn>();
        UIManager.Instance.CloseUI<UID_DayTextBG>();   
    }
}
