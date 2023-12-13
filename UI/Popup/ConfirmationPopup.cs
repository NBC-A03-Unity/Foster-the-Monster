using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopup : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text messageTxt;
    [SerializeField] private TMP_Text leftButtonTxt;
    [SerializeField] private TMP_Text rightButtonTxt;
    [Header("Button")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    public void SetTitle(string title)
    {
        titleTxt.text = title;
    }
    public void SetMessage(string message)
    {
        messageTxt.text = message;
    }

    public void SetLeftButtonText(string text)
    {
        leftButtonTxt.text = text;
    }

    public void SetRightButtonText(string text)
    {
        rightButtonTxt.text = text;
    }

    public void SetLeftButtonAction(Action action)
    {
        leftButton.onClick.RemoveAllListeners();
        leftButton.onClick.AddListener(new UnityEngine.Events.UnityAction(action));
    }

    public void SetRightButtonAction(Action action)
    {
        rightButton.onClick.RemoveAllListeners();
        rightButton.onClick.AddListener(new UnityEngine.Events.UnityAction(action));
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
