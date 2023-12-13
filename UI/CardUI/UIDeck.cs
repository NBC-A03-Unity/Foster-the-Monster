using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIDeck : MonoBehaviour
{
    private const float scrollSpeed = 300f;

    [SerializeField] private Button closeBtn;
    [SerializeField] public Transform DeckList;
    [SerializeField] private RectTransform content;
    public List<SelectCard> cardlist;

    private void Awake()
    {
        transform.SetParent(CardManager.Instance.DeckListController.transform);
        closeBtn.onClick.AddListener(OnCloseBtn);
        cardlist = new List<SelectCard>();
    }

 /*   private void OnEnable()
    {
       // transform.localPosition = new Vector3(0f, 400f, 0f);
    }
*/
    private void OnDisable()
    {
        foreach(Card card in cardlist)
        {
            ObjectManager.Instance.ReturnPool(card);
        }
        cardlist.Clear();
    }

  /*  private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        float yPos = DeckList.transform.localPosition.y + scroll * scrollSpeed;
        //content.sizeDelta
        yPos = yPos > 400 ? yPos : 400;
        DeckList.localPosition = new Vector3(0f, yPos, 0f);
    }*/

    public void AddCard(SelectCard card)
    {
        cardlist.Add(card);
        card.transform.SetParent(DeckList);
    }
    
    public void OnCloseBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        CardManager.Instance.DeckListController.ObjectSetFalse();
        UIManager.Instance.CloseUI<UIDeck>();
        
    }
}