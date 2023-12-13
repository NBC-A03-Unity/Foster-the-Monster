using System;

[Serializable]
public class EventFunc
{
    public EventFuncSO so;
    public int value;
    public Action OnApply(Action complete) 
    {
        return so.Apply(complete, value);
    }
}