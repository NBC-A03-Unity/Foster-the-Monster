using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UIHand : MonoBehaviour
{
    private const float DURATION = 0.3f;
    public Transform cardRoot;
    public Transform deckPosition;
    public Transform cardSlot;
    public Transform selectRoot;
    [SerializeField] private GameObject preventClick;

    public HandCard selectedCard;
    private HandCard Current;
    public bool isHide = false;
    [SerializeField] AssetReference handCardPrefabs;
    [SerializeField] GameObject text;
    private ObjectManager om;

    private const float HideY = -800f;
    private const float OpenY = 0;

    void Awake()
    {
        transform.SetParent(CardManager.Instance.HandCardController.transform);
        om = ObjectManager.Instance;
    }

    private void OnEnable()
    {
        HideHand();

    }


    private void OnDisable()
    {
        CardManager.Instance.HandCardController.ObjectSetFalse();
    }

    public void OnPreventClick()
    {
        preventClick.SetActive(true);
    }
    public void OffPreventClick()
    {
        preventClick.SetActive(false);
    }
    public void OnText()
    {
        text.SetActive(true);
    }
    public void OffText()
    {
        text.SetActive(false);
    }
    public void HideHand()
    {
        isHide = true;
        OnText();
        cardSlot.localPosition = new Vector3 (0, HideY, 0);

        foreach(HandCard hc in CardManager.Instance.HandCardController.cards)
        {
            hc.transform.position = deckPosition.position;
        }
        
    }
    public void OpenHand()
    {
        if (!isHide) return;
        isHide = false;
        OffText();
        cardSlot.localPosition = new Vector3(0, OpenY, 0);
        CardManager.Instance.SortCard();
    }
    public async Task<HandCard> CreatHandCard()
    {
        return await om.UsePool<HandCard>(handCardPrefabs, deckPosition.position, cardSlot);
    }


    public void ChangeSelectCard(HandCard card = null)
    {
        if(selectedCard != null) selectedCard.transform.SetParent(cardSlot);
        if (card != null) 
        {
            selectedCard = card;
            card.transform.SetParent(selectRoot);
            CardManager.Instance.HandCardController.PushHand(selectedCard.index);
        }
        else
        {
            CardManager.Instance.HandCardController.ResetHand();
        }
    }
    
}