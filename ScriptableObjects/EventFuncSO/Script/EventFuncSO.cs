using System;
using UnityEngine;

public abstract class EventFuncSO : ScriptableObject

{
    public abstract Action Apply(Action complete, int value = 0);

}