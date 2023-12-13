using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Enums;

public class DeckController : MonoBehaviour 
{
    private List<SelectCard> cards;
    private CardContainer cardContainer;
    private ObjectManager om;

    
    [SerializeField] private AssetReference cardPrefabs;

    private UIDeck view;
    private List<int> CardIDs;

    private void Awake()
    {
        om = ObjectManager.Instance;
        cardContainer = DataManager.Instance.CardContainer;
        cards = new List<SelectCard>();
    }
    
    private void LoadCardIDs()
    {
        CardIDs = cardContainer.ReturnDeckListSO();
        CardIDs.AddRange(CageManager.Instance.CageCardList());
    }
    
    private async Task LoadCardObj()
    {
        for (int i = 0; i < CardIDs.Count; i++)
        {
            SelectCard card = await om.UsePool<SelectCard>(cardPrefabs);
            await card.InitCard(CardIDs[i]);
            cards.Add(card);
        }
    }

    private List<SelectCard> CheckRarity(CardRarity cardRarity)
    {
        List<SelectCard> check = new List<SelectCard>();

        foreach(SelectCard sc in cards)
        {
            if (sc.cardSO.cardRarity.cardRarity == cardRarity)
            {
                check.Add(sc);
            }
        }

        return check;
    }

    private void SettingCheckCard(List<SelectCard> check)
    {
        foreach (SelectCard sc in check)
        {
            view.AddCard(sc);
            cards.Remove(sc);
        }
    }

    public async void UpdateDeckUI()
    {   
        view = UIManager.Instance.OpenUI<UIDeck>();
        LoadCardIDs();
        await LoadCardObj();

        for(int i =0; i< (int)CardRarity.EndPoint; i++)
        {
            List<SelectCard> complete = CheckRarity((CardRarity)i);
            SettingCheckCard(complete);
        }
        Canvas.ForceUpdateCanvases();
    }

    public void ObjectSetFalse()
    {
        foreach (SelectCard card in cards)
        {
            ObjectManager.Instance.ReturnPool(card.gameObject);
        }

        cards.Clear();
        CardIDs.Clear();
    }    

    
}
