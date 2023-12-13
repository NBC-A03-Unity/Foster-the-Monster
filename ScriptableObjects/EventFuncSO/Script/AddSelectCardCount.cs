using System;
public class AddSelectCardCount : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            DataManager.Instance.selectCount+= value;
            complete?.Invoke();
        };
    }
}
