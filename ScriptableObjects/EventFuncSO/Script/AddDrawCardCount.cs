using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventEffectData", menuName = "Data/EventEffect/SO", order = 0)]
public class AddDrawCardCount : EventFuncSO
{
    public override Action Apply(Action complete, int value = 0)
    {
        return () =>
        {
            DataManager.Instance.drawCardCount += value;
            complete?.Invoke();
        };
    }
}
