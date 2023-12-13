using System;
using static Enums;
public class AddPoolFruitEvent : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            if (DataManager.Instance.date > 14)
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Legend, CardType.Fruit, value);
            }
            else if(DataManager.Instance.date > 8)
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Epic, CardType.Fruit, value);
            }
            else if(DataManager.Instance.date > 3)
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Rare, CardType.Fruit, value);
            }
            else
            {
                CardManager.Instance.CardSelectController.AddCardPoolByRarity(CardRarity.Normal, CardType.Fruit, value);
            }
            complete?.Invoke();
        };
           
            
        
    }
}
