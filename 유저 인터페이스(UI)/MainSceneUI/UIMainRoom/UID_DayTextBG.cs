using TMPro;
using UnityEngine;

public class UID_DayTextBG : MonoBehaviour
{
    [SerializeField] private TMP_Text bossDday;
    [SerializeField] private TMP_Text goldDday;
    [SerializeField] private TMP_Text eventDday;
    [SerializeField] private TMP_Text goalType;
    [SerializeField] private TMP_Text currentType;


    private void OnEnable()
    {
        UpdateDayTxt();
    }
    public void UpdateDayTxt()
    {
        bossDday.text = (20 - DataManager.Instance.date).ToString();
        eventDday.text = (EventManager.Instance.ReturnNextEventDate() - DataManager.Instance.date).ToString();
        goldDday.text = (EventManager.Instance.ReturnNextGoldDate() - DataManager.Instance.date).ToString();
        goalType.text = DataManager.Instance.RequestMonsterTypes.ToString();
        currentType.text = DataManager.Instance.CompleteIDs.Count.ToString();
    }
}
