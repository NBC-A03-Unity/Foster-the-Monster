using System;

public class CageTemperatureEvent : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            foreach (CageMainController cc in CageManager.Instance.cageControllers)
            {
                cc.model.Temperature += value;
            }
            complete?.Invoke();
        };
    }
}