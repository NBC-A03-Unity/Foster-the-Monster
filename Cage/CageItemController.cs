using System;

public class CageItemController
{   
    private Cage model;
    private UICageItem view;
    private CageMainController controller;

    public CageItemController(CageMainController cage)
    {
        controller = cage;
        model = controller.model;   
    }
    public void SelectThisCage()
    {
        UIManager.Instance.CloseUI<UICageItem>();
        view = UIManager.Instance.OpenUI<UICageItem>();
        UpdateViewCageItem();
    }
    public async void UpdateViewCageItem()
    {
        CardSO[] cardSOs = new CardSO[3];
        Action[] actions = new Action[3];


        for (int i = 0; i < model.itemIDs.Length; i++)
        {
            if (model.itemIDs[i] == null)
            {
                cardSOs[i] = null;
                actions[i] = null;
                continue;
            }
            CardSO so = await ResourceManager.Instance.LoadResource<CardSO>($"CardSO_{model.itemIDs[i]}");
            cardSOs[i] = so;
            int index = i;
            actions[i] = () =>
            {
                model.itemIDs[index] = null;
                so.UnUse(controller);

            };
        }
        view.UpdateItem(cardSOs, actions);
    }
}