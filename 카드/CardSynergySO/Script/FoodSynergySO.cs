using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "FoodSynergy", menuName = "DataSO/CardSynergy/Default", order = 0)]
public class FoodSynergySO : SynergySO
{ 
    public FillersFoodType type;
    public Effect target;

    public override void Apply(CageCard cageCard , int value)
    {
        if (cageCard.cardSO.cardType != CardType.FillersFood) return;

        if ((cageCard.cardSO as FillersFoodCardSO).foodType != type) return;

        for (int i =0; i < cageCard.cardSO.cardEffects.Count; i++)
        {
            Effect effect = cageCard.cardSO.cardEffects[i].effect;
            
            if(effect == target)
            {
                cageCard.cardValue[i] += value;
            }
            
        }
    }
}