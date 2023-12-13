using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : UIToolTip
{
    private const float DURATION = 0.7f;
    [SerializeField] private Image stateBar;
    [SerializeField] private TMP_Text stateTxt;
    [SerializeField] private TMP_Text maxTxt;

    public void UpdateBar(float value, float max)
    {
        stateTxt.text = value.ToString();
        maxTxt.text = max.ToString();
        stateBar.DOFillAmount(value / max, DURATION);
    }
}
