using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventEffectData", menuName = "Data/EventEffect/CageTemperatureEvent2", order = 1)]

public class CageCleanlinessEvent : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            foreach (CageMainController cc in CageManager.Instance.cageControllers)
            {
                cc.model.Cleanliness += value;
            }
            complete?.Invoke();
        };
    }
}