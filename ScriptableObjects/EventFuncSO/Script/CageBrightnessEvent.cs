using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventEffectData", menuName = "Data/EventEffect/CageTemperatureEvent", order = 0)]

public class CageBrightnessEvent : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            foreach (CageMainController cc in CageManager.Instance.cageControllers)
            {
                cc.model.Brightness += value;
            }
            complete?.Invoke();
        };
    }
}