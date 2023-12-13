using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEvent : MonoBehaviour
{
    public const float DURATION = 1f;
    public EventSO eventSO;
    public TMP_Text eventName;
    public TMP_Text eventInfo;
    public List<Button> selectButtonList = new List<Button>();
    protected List<TMP_Text> selectTextList;

    
    protected virtual void Awake()
    {
        selectTextList = new List<TMP_Text>();

        foreach(Button btn  in selectButtonList)
        {
            selectTextList.Add(btn.GetComponentInChildren<TMP_Text>());
        }
    }

    public void Init(EventSO so, Action complete = null)
    {
        eventSO = so;
        eventName.text = eventSO.eventName;
        eventInfo.text = eventSO.eventInfo;
        

        for (int i = 0; i < selectButtonList.Count; i++)
        {
            if (i >= eventSO.selectEventInfo.Count)

            {
                break;
            }
            Button button = selectButtonList[i];
            EventFunc func = eventSO.selectEventFunc[i];
            Action action = null;
            button.onClick.AddListener(() => action = func.OnApply(complete));
            button.onClick.AddListener(ClearBtn);
            button.onClick.AddListener(CloseUI);
            button.onClick.AddListener(() => action?.Invoke());

            selectTextList[i].text = eventSO.selectEventInfo[i];
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Append(eventName.DOFade(1, DURATION));
        sequence.Append(eventInfo.DOFade(1, DURATION));
        for ( int i = 0; i < eventSO.selectEventInfo.Count; i++)
        {
            TMP_Text text = selectTextList[i];
            GameObject go = selectButtonList[i].gameObject;
            sequence.Append(text.DOFade(1, DURATION/3).OnComplete(() => go.SetActive(true)));
        }
        sequence.Play();
    }

    public void ColorChangeTMP(TMP_Text tmp)
    {
        Color color = tmp.color;
        color.a = 0;
        tmp.color = color;
    }
    public void CloseUI()
    {
        ColorChangeTMP(eventName);
        ColorChangeTMP(eventInfo);

        for (int i = 0; i < selectButtonList.Count; i++)
        {
            ColorChangeTMP(selectTextList[i]);
            selectButtonList[i].gameObject.SetActive(false);
        }
        UIManager.Instance.CloseUI<UIEvent>();
    }

    public void ClearBtn()
    {
        foreach(Button btn in selectButtonList)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}
