using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Enums;

public class HandController : MonoBehaviour
{
    public UIHand view;
    public List<HandCard> cards;
    private ObjectManager om;

    private void Awake()
    {
        om = ObjectManager.Instance;
        cards = new List<HandCard>();
    }

    public async Task SettingHand(List<int> drawCardIDs)
    {

        view = UIManager.Instance.OpenUI<UIHand>();
        
        if (cards.Count == drawCardIDs.Count)
        {
            return;
        }
        while (cards.Count < drawCardIDs.Count)
        {
            cards.Add(await view.CreatHandCard());
        }
        InitCard(drawCardIDs);

    }
    public void InitCard(List<int> cardSOs)
    {
        for (int i =0; i < cardSOs.Count; i++)
        {
            cards[i].InitCard(cardSOs[i]);
        }

        for(int i = cardSOs.Count; i < cards.Count; i++)
        {
            om.ReturnPool(cards[i]);
        }
    }

 
    public void RemoveHand(int i)
    {
        view.selectedCard = null;
        om.ReturnPool(cards[i]);
        cards.RemoveAt(i);
    }
    public void SortCardPos()
    {
        for(int i=0; i<cards.Count; i++)
        {
            cards[i].CaculatePos(i);
        }

    }

    public void MoveCardPosByStep(int i)
    {
        if (cards.Count == 0) return;

        view.OnPreventClick();
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Hand);
        if (i < cards.Count - 1)
        {
            cards[i].cardBack.SetActive(true);
            cards[i].ResetPosRotScaleAsync(() => MoveCardPosByStep(i + 1));
        }
        else
        {
            cards[i].ResetPosRotScaleAsync(() => view.OffPreventClick());
        }
    }

    public void MoveCardPosAll()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].ResetPosRotScale();
        }
    }


    public void PushHand(int index)
    {
        for (int i = 0; i < index; i++)
        {
            (cards[i]).PushedLeftCard();
        }

        for (int i = index + 1; i < cards.Count; i++)
        {
            (cards[i]).PushedRightCard();
        }
    }

    public HandCard ReturnSelectCard()
    {
        return view.selectedCard;
    }
 
    public void ResetHand()
    {
        foreach (HandCard card in cards)
        {
            card.ResetPosRotScale();
        }
    }
    public void ObjectSetFalse()
    {
        foreach (HandCard card in cards)
        {
            ObjectManager.Instance.ReturnPool(card);
        }
        cards.Clear();
    }
}
