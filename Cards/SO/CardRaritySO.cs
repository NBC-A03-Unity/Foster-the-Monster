using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "CardRarity", menuName = "DataSO/CardRarity/Default", order = 0)]
public class CardRaritySO : ScriptableObject
{ 
    public Sprite cardImageByRarity;
    public Sprite cardImageGem;
    public CardRarity cardRarity;
}
