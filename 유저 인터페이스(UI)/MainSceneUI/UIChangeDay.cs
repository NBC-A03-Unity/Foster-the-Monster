using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIChangeDay : MonoBehaviour
{
    private const float DURATION = 1.5f;
    [SerializeField] private Image changeDayUI;
    [SerializeField] private TextMeshProUGUI changeDayTxt;


    private void OnEnable()
    {
        changeDayTxt.text = "Day " + DataManager.Instance.date.ToString();
        Sequence sequence = DOTween.Sequence();
        sequence.Join(changeDayTxt.DOFade(1, DURATION));
        sequence.Join(changeDayUI.DOFade(1, DURATION));
        sequence.Play().OnComplete(() =>
        {
            sequence = DOTween.Sequence();
            sequence.Join(changeDayTxt.DOFade(0, DURATION));
            sequence.Join(changeDayUI.DOFade(0, DURATION));
            sequence.Play().OnComplete(() =>
            {
                UIManager.Instance.CloseUI<UIChangeDay>();
                EventManager.Instance.Load(()=> Complete());
            });
        });
    }

    public void Complete()
    {
        DataManager.Instance.EndDayComplete();
        if (DataManager.Instance.resultCount == 0)
        {
            DataManager.Instance.SaveData();
        }
    }
}