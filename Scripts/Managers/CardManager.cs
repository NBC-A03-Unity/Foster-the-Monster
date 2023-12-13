using System.Collections.Generic;
using System.Threading.Tasks;

public class CardManager : Singleton<CardManager>
{
    public DeckController DeckListController;
    public HandController HandCardController;
    public SelectController CardSelectController;

    ResourceManager rm;

    protected override void Awake()
    {
        base.Awake();
        DeckListController = GetComponent<DeckController>();
        HandCardController = GetComponent<HandController>();
        CardSelectController = GetComponent<SelectController>();
        rm = ResourceManager.Instance;
    }

    private void Start()
    {
        CardSelectController.InitCardSetting();
    }
    public void CloseUIHand()
    {
        if (HandCardController.view == null)
        {
            HandCardController.view = UIManager.Instance.OpenUI<UIHand>();
        }
        
        HandCardController.view.HideHand();
        HandCardController.view.OffText();
    }

    public async void CardSelect()
    {
        if (DataManager.Instance.selectCount > 0)
        {
            await CardSelectController.SettingCard();
        }
    }

    public void CheckResult()
    {
        if (HandCardController.view == null)
        {
            HandCardController.view = UIManager.Instance.OpenUI<UIHand>();
        }            
        HandCardController.view.OnText();
    }
    public async void LoadOrDrawCard()
    {
        if (DataManager.Instance.resultCount != 0) 
        {
            CloseUIHand();
            return;
        }
        
        List<int> drawCardIDs = DrawCardID();
        await HandCardController.SettingHand(drawCardIDs);
        HandCardController.view.OpenHand();
    }

    public void SortCard()
    {
        HandCardController.SortCardPos();
        HandCardController.MoveCardPosByStep(0);
    }

    public void NonSoundSortCard()
    {
        HandCardController.SortCardPos();
        HandCardController.MoveCardPosAll();
    }
    public List<int> DrawCardID()
    {
        List<int> cardIDs = DataManager.Instance.CardContainer.ReturnHand();

        if (!DataManager.Instance.todayCardSelect || cardIDs.Count == 0)
        {
            DataManager.Instance.todayCardSelect = true;
            while (cardIDs.Count < DataManager.Instance.drawCardCount)
            {
                int? id = DataManager.Instance.CardContainer.DeckToHand();
                if (id == null) break;
                cardIDs.Add((int)id);
            }
        }

        return cardIDs;
    }

    public void UseCard(int index)
    {
        HandCardController.RemoveHand(index);
        DataManager.Instance.CardContainer.RemoveHand(index);

        if (DataManager.Instance.CardContainer.HandCount == 0)
        {
            UIManager.Instance.CloseUI<UIHand>();
            LoadOrDrawCard();
        }
        else
        {
            NonSoundSortCard();
        }
    }

}