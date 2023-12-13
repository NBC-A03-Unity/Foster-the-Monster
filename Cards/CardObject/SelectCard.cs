using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Utility;

public class SelectCard : Card, IPointerEnterHandler, IPointerExitHandler
{
    protected const float DURATION = 0.3f;

    protected Vector2 currentPos;

    protected Vector2 initialPos;
    protected Vector2 initialScale = new Vector2(1f,1f);
    protected Quaternion initialRot;
    protected Vector2 enlargeScale = new Vector2(1.7f, 1.7f);
    public bool isClickprevent;

    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardInfo;
    [SerializeField] private Image cardRarityImage;
    [SerializeField] private Image cardGem;
    [SerializeField] private TMP_Text cardtype;

    protected virtual void OnEnable()
    {
        transform.localScale = initialScale;
    }
    public override async Task InitCard(int id)
    {
        await base.InitCard(id);
        cardName.text = cardSO.cardName;
        isClickprevent = false;
        CardInfo();
        cardRarityImage.sprite = cardSO.cardRarity.cardImageByRarity;
        cardGem.sprite = cardSO.cardRarity.cardImageGem;
        cardtype.text = cardSO.ReturnCardType();
    }
    public void CardInfo()
    {
        sb.Clear();
        sb.AppendLine(cardSO.cardInfo);
        for (int i = 0; i < cardSO.cardEffects.Count; i++)
        {
            cardSO.cardEffects[i].MakeCardEffectTxt(cardSO.cardEffects[i].value);
        }
        cardInfo.text = sb.ToString();
    }
    public void PosRotInit(Vector3 pos, Quaternion rot)
    {
        initialPos = pos;
        initialRot = rot;
    }
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (isClickprevent) return;
        transform.DOComplete();
        transform.DORotate(Vector3.zero, DURATION);
        transform.DOScale(enlargeScale, DURATION);        

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (isClickprevent) return;
        transform.DOComplete();
        transform.DOScale(initialScale, DURATION);
    }
}
