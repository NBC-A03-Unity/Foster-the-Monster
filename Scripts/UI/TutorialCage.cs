using UnityEngine;

public class TutorialCage : MonoBehaviour
{
    UICageInfo UICageInfo;
    UICage UICage;
    UICageCard UICageCard;
    UICageItem UICageItem;
    UIFosMonInfo UIFosMonInfo;
    private void Awake()
    {
        UICageInfo = UIManager.Instance.OpenUI<UICageInfo>();
        UICage = UIManager.Instance.OpenUI<UICage>();
        UICageCard = UIManager.Instance.OpenUI<UICageCard>();
        UICageItem = UIManager.Instance.OpenUI<UICageItem>();
        UIFosMonInfo = UIManager.Instance.OpenUI<UIFosMonInfo>();
        UITutorialCage tutorial = UIManager.Instance.OpenUI<UITutorialCage>();
        tutorial.TutorialCage = gameObject;
        if (!DataManager.Instance.MainSceneTutorial)
        {
            tutorial.Canvass[1] = UICageInfo.GetComponent<Canvas>();
            tutorial.Canvass[2] = UIFosMonInfo.GetComponent<Canvas>();
            tutorial.Canvass[3] = tutorial.Canvass[2];
            tutorial.Canvass[4] = UICageItem.GetComponent<Canvas>();
            tutorial.Canvass[5] = UICageCard.GetComponent<Canvas>();
        }
    }

    private void OnDestroy()
    {
        UIManager.Instance.CloseUI<UICageInfo>();
        UIManager.Instance.CloseUI<UICageCard>();
        UIManager.Instance.CloseUI<UICageItem>();
        UIManager.Instance.CloseUI<UIFosMonInfo>();
    }
}
