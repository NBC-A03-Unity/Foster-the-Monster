using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Utility;
public class CageCardController
{
    private const float MAXCARD = 4;
    private Cage model;
    private UICageCard view;
    private CageMainController controller;
    private ResourceManager resourceManager;
    private ObjectManager objectManager;
    public CageCardController(CageMainController cage)
    {
        controller = cage;
        model = cage.model;
        resourceManager = ResourceManager.Instance;
        objectManager = ObjectManager.Instance;
    }
    public void SelectThisCage(Action action)
    {
        UIManager.Instance.CloseUI<UICageCard>();
        view = UIManager.Instance.OpenUI<UICageCard>();

        CheckResultVisibility(action);
        view.UpdateCardCount();

        for (int i = 0; i < model.cardIDs.Count; i++)
        {
            AsyncLoadCard(model.cardIDs[i], model.cardEffectValues[i], model.cardSuccess[i]);
        }
        
    }
    public void CheckCardSuccess()
    {
        for (int i = 0; i < model.cardIDs.Count; i++)
        {
            AsyncCheckCardSuccess(i);
        }
    }
    public async void Usecard()
    {
        for(int i =0; i<model.cardIDs.Count; i++) 
        {
            CardSO so = await resourceManager.LoadResource<CardSO>($"CardSO_{model.cardIDs[i]}");
            so.UseCard(controller, model.cardEffectValues[i]);
        }

        foreach (CageCard cc in view.CageCards)
        {
            cc.Clear();
        }

        model.isUse = false;
        model.CardClear();
        view.CageCards.Clear();
    }
    public bool CheckCard(CardSO so)
    {
        if (model.monster == null || model.monster._data == null)
        {
            CheckPopUp(4007, 3007);
            return false;
        }
        if (model.cardIDs.Count == MAXCARD)
        {
            CheckPopUp(4009, 3009);
            return false;
        }

        model.cardIDs.Add(so.cardId);
        AsyncInitCard(so.cardId);
        return true;
    }
    public void UpdateCardInfo()
    {
        for(int i=0; i<  view.CageCards.Count; i++)
        {
            view.CageCards[i].UpdateCardInfo(model.cardEffectValues[i]);
        }
    }

    private void CheckResultVisibility(Action action)
    {
        if (model.isUse)
        {
            action += () => { model.isUse = false; };
            view.OnResultBtn(action);
        }
        else
        {
            view.ClearBtn();
        }

    }
    private void ApplySynergy(CageCard card)
    {
        card.ApplySynergyToList(view.CageCards);
        foreach (CageCard data in view.CageCards)
        {
            data.ApplySynergyToCard(card);
        }
    }
    private async void AsyncCheckCardSuccess(int i)
    {
        CardSO so = await resourceManager.LoadResource<CardSO>($"CardSO_{model.cardIDs[i]}");
        model.cardSuccess[i] = so.CheckSuccessLevel(controller, model.cardEffectValues[i]);
    }
    private async void AsyncLoadCard(int id, List<int> effectValue, string Sucess)
    {  
        CardSO so = await resourceManager.LoadResource<CardSO>($"CardSO_{id}");
        CageCard card = await objectManager.UsePool<CageCard>("CageCard", view.cardListTransform);
        card.LoadData(so, effectValue, Sucess, model.isUse);
        view.UpdateUseCard(card);
    }
    private async void AsyncInitCard(int id)
    {
        CardSO so = await resourceManager.LoadResource<CardSO>($"CardSO_{id}");
        CageCard card = await objectManager.UsePool<CageCard>("CageCard", view.cardListTransform);

        List<int> effectValue = new List<int>();
        card.InitData(so, effectValue);
        ApplySynergy(card);
        model.cardEffectValues.Add(effectValue);
        model.cardSuccess.Add("NotUse");
        view.UpdateUseCard(card);
    }
}