using System;

public class AddCardOptions : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            DataManager.Instance.selectCardOptions += value;
            complete?.Invoke();
        };
        
    }
}