using System;
public class BossRunEventSO : EventFuncSO
{

    public override Action Apply(Action complete, int value = 0)
    {
        return () => EventManager.Instance.WaringEvent("BossRunGameOver");
    }
}
