using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICageCard : UIToolTip
{
    [SerializeField] private Button resultButton;
    [SerializeField] private GameObject result;
    [SerializeField] private TMP_Text useCardCount;
    [SerializeField] public Transform cardListTransform;
    public List<CageCard> CageCards { get; private set; }

    public void UpdateCardCount()
    {
        useCardCount.text = CageCards.Count.ToString();
    }

    private void OnEnable()
    {
        CageCards = new List<CageCard>();
    }

    private void OnDisable()
    {
        foreach (var Card in CageCards)
        {
            ObjectManager.Instance.ReturnPool(Card);
        }
        CageCards.Clear();
        UpdateCardCount();
    }
    public void UpdateUseCard(CageCard card)
    {
        CageCards.Add(card);
        useCardCount.text = CageCards.Count.ToString();
    }


    public void OnResultBtn(Action action)
    {
        resultButton.onClick.RemoveAllListeners();
        resultButton.gameObject.SetActive(true);
        result.gameObject.SetActive(true);
        resultButton.onClick.AddListener(() => action.Invoke());
        resultButton.onClick.AddListener(UpdateCardCount);
        resultButton.onClick.AddListener(ClearBtn);
    }
    public void ClearBtn()
    {
        resultButton.onClick.RemoveAllListeners();
        result.SetActive(false);        
    }

}
