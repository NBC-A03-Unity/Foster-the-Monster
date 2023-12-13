using System;

public class ResultReportingEvent : EventFuncSO
{

    public override Action Apply(Action complete, int value = 0)
    {
        if (DataManager.Instance.CompleteIDs.Count >= DataManager.Instance.RequestMonsterTypes)
        {
            DataManager.Instance.RequestMonsterTypes += 2;
            return complete;
        }
        else
        {
            DataManager.Instance.GameOver = true;
            return () => EventManager.Instance.WaringEvent("DefaultGameOver");
        }
    }
}