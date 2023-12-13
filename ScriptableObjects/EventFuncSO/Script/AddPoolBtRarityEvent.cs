using System;
using static Enums;
public class AddPoolBtRarityEvent : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            if (DataManager.Instance.date > 14)
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Legend, value : value);
            }
            else if(DataManager.Instance.date > 8)
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Epic, value: value);
            }
            else if(DataManager.Instance.date > 3)
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Rare, value: value);
            }
            else
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Normal, value: value);
            }
            complete?.Invoke();
        };
    }
}
