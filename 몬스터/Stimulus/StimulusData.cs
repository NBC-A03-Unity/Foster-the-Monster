using System;
using UnityEngine;
using static Enums;

[Serializable]
public class StimulusData
{
    public StimulusObjectType objectType;
    public StimulusType type;
    public FillersFoodType foodType;
    public Transform transform;
    public Sprite icon;
    [Range(1, 5)] public int range=1;
    public bool hasDestroyTime;
    public float stimulusDuration;
    public bool isIdentifiable;
}
