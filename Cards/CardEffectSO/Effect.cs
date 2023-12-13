using System;
using UnityEngine;

[Serializable]
public abstract class Effect : ScriptableObject
{
    [Header("Effect")]
    public string effectName;
    public int effectid;
    public abstract void ApplyEffect(CageMainController cage,int value);

    public void RemoveEffect(CageMainController cage, int value)
    {
        ApplyEffect(cage, -value);
    }
}