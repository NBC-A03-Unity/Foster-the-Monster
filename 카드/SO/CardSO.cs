using System.Collections.Generic;
using UnityEngine;
using static Enums;
using static Utility;

[CreateAssetMenu(fileName = "CardData", menuName = "DataSO/Card/Default", order = 0)]
public class CardSO : ScriptableObject
{
    [Header("Default")]
    public int cardId;
    public string cardName;
    public string cardInfo;
    public Sprite cardImage;
    public CardRaritySO cardRarity;
    public List<CardEffect> cardEffects;
    public List<CardSynergy> cardSynergies;
    public CardType cardType;

    public string CardInfoUpdate(List<int> cardValue)
    {
        sb.Clear();
        for (int i = 0; i < cardEffects.Count; i++)
        {
            cardEffects[i].MakeCardEffectTxt(cardValue[i]);
        }

        return sb.ToString();
    } 
    public string CardInfoUpdate()
    {
        sb.Clear();
        for (int i = 0; i < cardEffects.Count; i++)
        {
            cardEffects[i].MakeCardEffectTxt(cardEffects[i].value);
        }

        return sb.ToString();
    }
    public virtual string ReturnCardType()
    {
        return cardType.ToString();
    }
    public virtual string CheckSuccessLevel(CageMainController cage, List<int> value)
    {
        cage.fosMonInfoController.UpdateAchievement(3);
        return "Good";
    }

    public virtual void UseCard(CageMainController cage)
    {
        for (int i = 0; i < cardEffects.Count; i++)
        {
            cardEffects[i].effect.ApplyEffect(cage, cardEffects[i].value);
        }
    }
    public virtual void UnUse(CageMainController cage)
    {
        for (int i = 0; i < cardEffects.Count; i++)
        {
            cardEffects[i].effect.RemoveEffect(cage, cardEffects[i].value);
        }
    }
    public virtual void UseCard(CageMainController cage, List<int> value)
    {
        for(int i =0; i < cardEffects.Count; i++)
        {
            cardEffects[i].effect.ApplyEffect(cage, value[i]);
        }

    }
    
    
}
