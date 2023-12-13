using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static Enums;
using static Utility;

public class SelectController : MonoBehaviour
{
    private UISelect view;    
    private List<SelectCard> cards;
    private List<Button> buttons;

    [SerializeField] private AssetReference[] cardLists = new AssetReference[4];

    private CardManager cm;

    private void OnEnable()
    {
        cards = new List<SelectCard>();
        buttons = new List<Button>();
        cm = CardManager.Instance;
    }
 
    public async Task SettingCard()
    {
        
        await SettingCardObj();
        InitCardSO();
    }

    public async void InitCardSetting()
    {
        if (DataManager.Instance.SelectCardWeightPool.Count != 0) return;
        
        AssetReference address = cardLists[(int)CardRarity.Normal];
        AssetReferenceList list = await ResourceManager.Instance.LoadResource<AssetReferenceList>(address);
        List<AssetReference> ARList = list.list;

        foreach (AssetReference asset in list.list)
        {
            CardSO so = await ResourceManager.Instance.LoadResource<CardSO>(asset);

            AddCardWeightByID(so.cardId, 3);
            
            
            DataManager.Instance.CardContainer.AddGraveyard(so);

        }
    }

    public void AddCardPool()
    {
        if (DataManager.Instance.date > 4)
        {
            AddCardPoolByRarity(CardRarity.Normal, CardType.Item);
        }

        if (DataManager.Instance.date < 7)
        {
            AddCardPoolByRarity(CardRarity.Rare, value: 2);
        }
        else if (DataManager.Instance.date < 13)
        {
            AddCardPoolByRarity(CardRarity.Epic, value: 2);
        }
        else
        {
            AddCardPoolByRarity(CardRarity.Epic, value: 3);
        }
    }
    private void ClearAllButton()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        buttons.Clear();
    }
    private void ObjectSetFalse()
    {
        UIManager.Instance.CloseUI<UISelect>();
        ClearAllButton();
        foreach (SelectCard card in cards)
        {
            ObjectManager.Instance.ReturnPool(card);
        }
    }
    private async void InitCardSO()
    {
        foreach (SelectCard card in cards)
        {
            int id = SelectCardId();
            if (id == 0)
            {
                return;
            }
            await card.InitCard(id);
            Button button = card.GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            buttons.Add(button);
            button.onClick.AddListener(() => OnCardSelectBtn(card));
            view.SettingCompleted();
        }

        foreach (SelectCard card  in cards)
        {
            card.transform.DOScale(Vector3.one, 0.5f);
        }

        
    }
    private async Task SettingCardObj()
    {
        ClearAllButton();
        view = UIManager.Instance.OpenUI<UISelect>();
        while (cards.Count != DataManager.Instance.selectCardOptions)
        {
            SelectCard obj = await view.AddCardObject();
            cards.Add(obj);
        }    
        
        foreach(SelectCard card in cards)
        {
            card.transform.DOComplete();
            card.transform.localScale = Vector3.zero;
        }
        await Task.Delay(500);

    }
    private async void OnCardSelectBtn(SelectCard card)
    {
        DataManager.Instance.selectCount--;
        view.StopLoading();
        DataManager.Instance.CardContainer.AddDeck(card.cardSO);
        UIManager.Instance.CloseUI<UISelect>();
        if (DataManager.Instance.selectCount > 0)
        {
            await SettingCard();
        }
        else
        {
            
            ObjectSetFalse();
            cards.Clear();
        }
    }
    private void AddCardWeightByID(int cardid,int value)
    {
        if (!DataManager.Instance.SelectCardWeightPool.ContainsKey(cardid))
        {
            DataManager.Instance.SelectCardWeightPool.Add(cardid, value);
        }
        else
        {
            DataManager.Instance.SelectCardWeightPool[cardid] += value;
        }
        DataManager.Instance.weight += value;
    }
    private int SelectCardId()
    {
        int cardId=0;

        int targetWeight = Math.Abs(RandomInt() % DataManager.Instance.weight);

        int currentWeight = 0;

        foreach (KeyValuePair<int,int> cardWeight in DataManager.Instance.SelectCardWeightPool)
        {
            currentWeight += cardWeight.Value;
            
            if (currentWeight >= targetWeight)
            {
                cardId = cardWeight.Key;
                break;
            }
        }
        return cardId;
    }
    public async void AddCardPoolByRarity(CardRarity rarity, CardType type = CardType.EndPoint, int value = 1)
    {
        AssetReference address = cardLists[(int)rarity];
        AssetReferenceList list = await ResourceManager.Instance.LoadResource<AssetReferenceList>(address);
        List<AssetReference> ARList = list.list;

        foreach(AssetReference asset in list.list)
        {
            CardSO so = await ResourceManager.Instance.LoadResource<CardSO>(asset);

            if (type == CardType.EndPoint)
            {
                AddCardWeightByID(so.cardId, value);
            }
            else if (type == so.cardType) 
            {
                AddCardWeightByID(so.cardId, value);
            }
            
        }
    }
}