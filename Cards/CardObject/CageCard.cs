using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CageCard : Card
{
    public List<int> cardValue;

    [SerializeField] private TextMeshProUGUI isSusccessTxt;
    [SerializeField] private TextMeshProUGUI cardInfo;
    public void LoadData(CardSO so, List<int> value, string Sucess, bool isUse)
    {
        cardSO = so;
        cardValue = value;
        cardImage.sprite = cardSO.cardImage;
        cardInfo.text = cardSO.CardInfoUpdate(cardValue);
        UpdateCardText(Sucess, isUse);
    }

    public void InitData(CardSO so, List<int> value)
    {
        cardSO = so;
        cardValue = value;
        foreach (CardEffect effect in cardSO.cardEffects)
        {
            value.Add(effect.value);
        }

        cardImage.sprite = cardSO.cardImage;
        cardInfo.text = cardSO.CardInfoUpdate(cardValue);
        isSusccessTxt.text = cardSO.ReturnCardType();
    }

    public void UpdateCardText(string Sucess, bool isUse)
    {
        if (isUse)
        {
            isSusccessTxt.text = Sucess;

        }
        else
        {
            isSusccessTxt.text = cardSO.ReturnCardType();
        }

    }

    public void ApplySynergyToList(List<CageCard> list)
    {
        foreach(CageCard card in list)
        {
            ApplySynergyToCard(card);
        }  
        
    }
    public void ApplySynergyToCard(CageCard card)
    {
        foreach (CardSynergy synergy in cardSO.cardSynergies)
        {
            synergy.ApplySynergy(card);
        }
    }

    public void UpdateCardInfo(List<int> cardEffectValues)
    {
        cardInfo.text = cardSO.CardInfoUpdate(cardEffectValues);
    }

    public void Clear()
    {
        DataManager.Instance.CardContainer.AddGraveyard(cardSO);
        ObjectManager.Instance.ReturnPool(gameObject);
    }   
}