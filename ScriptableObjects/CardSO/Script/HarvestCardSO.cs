using System.Collections.Generic;
using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "CardData", menuName = "DataSO/Card/Harvest", order = 2)]
public class HarvestCardSO : CardSO
{
    [Header("Harvest")]
    public HarvestType harvestType;

    public override string CheckSuccessLevel(CageMainController cage, List<int> value)
    {
        MonsterData monster = cage.model.monster;
        MonsterSO so = monster._data;
        if (so.harvestType == harvestType)
        {
            UseCard(cage, value);
            cage.fosMonInfoController.UpdateAchievement(10);
            return "Excellent";
        }
        else
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = 0;
            }
            UseCard(cage, value);
            return "Fail";
        }
    }

    public override void UseCard(CageMainController cage, List<int> value)
    {
        for (int i = 0; i < cardEffects.Count; i++)
        {
            cardEffects[i].effect.ApplyEffect(cage, value[i]);
        }

    }
}
