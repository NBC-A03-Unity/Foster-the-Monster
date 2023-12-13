using System;
using UnityEngine;
using UnityEngine.UI;

public class UICageItem : UIToolTip
{
    [SerializeField] private Transform itemListTransform;
    [SerializeField] public Card[] ItemImages;
    private Button[] itemButtons;

    protected override void Awake()
    {
        base.Awake();
        itemButtons = itemListTransform.GetComponentsInChildren<Button>();
    }
    public void UpdateItem(CardSO[] itemSOs, Action[] actions)
    {
        for (int i = 0; i < ItemImages.Length; i++)
        {
            if (itemSOs[i] != null)
            {
                InitButton(i, actions[i]);
                ItemImages[i].InitCard(itemSOs[i].cardId);
                ItemImages[i].InitItemCard();
                ItemImages[i].gameObject.SetActive(true);
            }
            else
            {
                ItemImages[i].gameObject.SetActive(false);
                itemButtons[i].onClick.RemoveAllListeners();
            }
        }

    }
    public void InitButton(int index, Action action)
    {
        itemButtons[index].onClick.AddListener(() =>
        {
            action += () =>
            {
                itemButtons[index].onClick.RemoveAllListeners();
                ItemImages[index].gameObject.SetActive(false);
                ItemImages[index].RemoveCard();
            };
            ButtonPopup(action);
        });

    }
    public void ButtonPopup(Action action)
    {

        int exitPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2007 : 1007;

        UIManager.Instance.OpenConfirmationPopup(
            exitPopupKey,
            () =>
            {
                action?.Invoke();
                UIManager.Instance.CloseUI<ConfirmationPopup>();

            },
            () =>
            {
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );

    }
}
