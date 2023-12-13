using System.Collections.Generic;
using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "CardData", menuName = "DataSO/Card/Food", order = 1)]
public class FillersFoodCardSO : CardSO
{
    [Header("FillersFood")]
    public FillersFoodType foodType;

    public override string CheckSuccessLevel(CageMainController cage, List<int> value)
    {
        MonsterSO so = cage.model.monster._data;
       if (so.preferFoodType == foodType)
        {
            UseCard(cage, value);

            cage.fosMonInfoController.UpdateAchievement(20);
            return "Excellent";
        }
        else if(so.hateFoodType == foodType)
        {
            for(int i =0; i< value.Count; i++)
            {
                value[i] = 0;
            }

            UseCard(cage, value);
            return "Fail";
        }
        else
        {
            UseCard(cage, value);
            cage.fosMonInfoController.UpdateAchievement(5);
            return "Good";
        }
    }

    public override void UseCard(CageMainController cage, List<int> value)
    {
        for (int i = 0; i < cardEffects.Count; i++)
        {
            cardEffects[i].effect.ApplyEffect(cage, value[i]);
        }

    }

    public override string ReturnCardType()
    {
        return foodType.ToString();
    }
} 
