using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class CardContainer
{
    public int HandCount { get { return hand.Count; } }
    [JsonProperty] private List<int> hand;
    [JsonProperty] private List<int> graveyard;
    [JsonProperty] private Stack<int> deck;

    public CardContainer()
    {
        hand = new List<int>(5);
        graveyard = new List<int>(17);
        deck = new Stack<int>(17);
    }

    public void ReshuffleDeck()
    {
        while (graveyard.Count > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, graveyard.Count);
            deck.Push(graveyard[randIndex]);
            graveyard.RemoveAt(randIndex);
        }
    }
    public void RemoveAllHand()
    {
        foreach (int cardSO in hand)
        {
            graveyard.Add(cardSO);
        }
        hand.Clear();
    }
    public void RemoveHand(int index)
    {
        hand.RemoveAt(index);
    }

    public void AddGraveyard(CardSO so)
    {
        graveyard.Add(so.cardId);
    }
    public void AddDeck(CardSO so)
    {
        deck.Push(so.cardId);
    } 
    public int? DeckToHand()
    {
        if (deck.Count == 0)
        {
            ReshuffleDeck();
        }

        if (deck.Count == 0)
        {
            return null;
        }

        int id = deck.Pop();
        hand.Add(id);
        return id;
    }
    public List<int> ReturnDeckListSO()
    {
        List<int> cardSOs = new List<int>();

        foreach(int so in hand)
        {
            cardSOs.Add(so);
        }

        foreach (int so in deck)
        {
            cardSOs.Add(so);
        }

        foreach(int so in graveyard)
        {
            cardSOs.Add(so);
        }

        return cardSOs;
    }
    public List<int> ReturnHand()
    {
        List<int> list = new List<int>();

        foreach (int so in hand)
        {
            list.Add(so);
        }

        return list;
    }
    
}