using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : SelectCard, IPointerDownHandler, IPointerUpHandler
{
    #region Const
    private const float MAX_PRESS_TIME = 0.3f;
    private const float X_PUSH_VALUE = 80f;
    private const float SPREAD_RANGE_ANGLE = 30f;
    private const float ROTATION_RADIUS = 900f;
    #endregion    
    private bool isPressed;
    private bool isAutoDrag;
    private bool isSuccess;
    private float pressTime;
    public int index;
    

    [SerializeField] public GameObject cardBack;
    private Vector3 deckPos;
    private HandController hand;
    public override async Task InitCard(int id)
    {
        hand = CardManager.Instance.HandCardController;
        gameObject.SetActive(true);
        cardBack.SetActive(true);
        await base.InitCard(id);
    }

    public void ResetPosRotScaleAsync(Action action = null)
    {
        isPressed = false;
        isAutoDrag = false;

        isClickprevent = true;
        transform.SetSiblingIndex(index);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOLocalMove(initialPos, DURATION));
        sequence.Join(transform.DOLocalRotate(initialRot.eulerAngles, DURATION));
        sequence.Join(transform.DOScale(initialScale, DURATION));
        sequence.Play().OnComplete(
            () =>
            {
                action?.Invoke();
                cardBack.SetActive(false);
                isClickprevent = false;
            }
            );
    }

    public void ResetPosRotScale()
    {
        if (isClickprevent) return;
        cardBack.SetActive(false);
        isPressed = false;
        isAutoDrag = false;
        transform.localPosition = initialPos;
        transform.localRotation = initialRot;
        transform .localScale = initialScale;
        transform.SetSiblingIndex(index);
    }

    public void PushedLeftCard()
    {
        PushCard(-X_PUSH_VALUE);
    }
    public void PushedRightCard()
    {
        PushCard(X_PUSH_VALUE);
    }
    private void Awake()
    {
        initialScale = transform.localScale;
    }
    private void Update()
    {
        if (isPressed)
        {
            pressTime += Time.deltaTime;
        }

        if ((isPressed || isAutoDrag))
        {
            DragCard();
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        enlargeScale = new Vector2(2.3f, 2.3f);
        hand = CardManager.Instance.HandCardController;
        isPressed = false;
        isAutoDrag = false;
        isSuccess = false;
        pressTime = 0.0f;
        cardBack.SetActive(true);
    }

    #region MouseEvent
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isClickprevent) return;

        base.OnPointerEnter(eventData);
        hand.view.ChangeSelectCard(this);
        transform.localPosition = new Vector3(initialPos.x, 50, 0);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (isClickprevent) return;
        if (isSuccess) return;
        base.OnPointerExit(eventData);
        hand.view.ChangeSelectCard();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isClickprevent) return;
        if (isPressed && !isAutoDrag)
        {
            if (pressTime < MAX_PRESS_TIME)
            {
                isAutoDrag = true;
                isPressed = false;
            }
            else
            {
                ResetPosRotScale();
                UseCard(eventData);
            }

            pressTime = 0;
        }
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isClickprevent) return;

        if (hand.ReturnSelectCard() ==  this)
        {
            if (isAutoDrag)
            {
                ResetPosRotScale();
                UseCard(eventData);
            }
            else
            {
                isPressed = true;
            }
        }

    }
    #endregion  

    private void DragCard()
    {
        if (isClickprevent) return;
        transform.position = Input.mousePosition;
    }
    private void PushCard(float x)
    {
        Vector3 pos = (Vector3)initialPos + new Vector3(x, 0, 0);
        transform.localPosition = pos;
        transform.rotation = initialRot;
    }
    private void UseCard(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint((RectTransform)hand.view.cardSlot, eventData.position))
        {
            CageManager.Instance.AddCardToCage(cardSO, index);

        }
    }
    
    
    public void CaculatePos(int i)
    {
        index = i;
        int count = DataManager.Instance.CardContainer.HandCount;
   
        float t = index == 0 ? 0 : (float)index / (count-1);
        float angle;

        if (count < 10)
        {
            float minAngle = SPREAD_RANGE_ANGLE / 5;
            int length = count / 2; 

            angle = Mathf.Lerp(minAngle * length, -minAngle * length, t);
        }
        else
        {
            angle = Mathf.Lerp(SPREAD_RANGE_ANGLE, -SPREAD_RANGE_ANGLE, t);
        }

        Vector3 cardPos = new Vector3(0, -ROTATION_RADIUS, 0) + Quaternion.Euler(0, 0, angle) * new Vector3(0, ROTATION_RADIUS, 0);

        Quaternion cardRot = Quaternion.Euler(0, 0, angle);

        PosRotInit(cardPos, cardRot);       
    }
}