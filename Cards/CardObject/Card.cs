using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class Card : MonoBehaviour
{
    [SerializeField] protected Image cardImage;
    [SerializeField] private TMP_Text text;
    public int cardID;
    public CardSO cardSO;

    public virtual async Task InitCard(int id)
    {

        cardID = id;
        cardSO = await ResourceManager.Instance.LoadResource<CardSO>($"CardSO_{cardID}");
        cardImage.sprite = cardSO.cardImage;
    }

    public void InitItemCard()
    {
        text.text = cardSO.CardInfoUpdate();
    }

    public void RemoveCard()
    {
        cardSO = null;
        cardImage.sprite = null;
    }
}