using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleConfirmationPopup : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text messageTxt;
    [Header("Button")]
    [SerializeField] private TMP_Text buttonTxt;
    [SerializeField] private Button button;

    public void SetTitle(string title)
    {
        titleTxt.text = title;
    }

    public void SetMessage(string message)
    {
        messageTxt.text = message;
    }

    public void SetButtonText(string text)
    {
        buttonTxt.text = text;
    }

    public void SetButtonAction(Action action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(action));
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
