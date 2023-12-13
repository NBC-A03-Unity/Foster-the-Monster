using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DateData", menuName = "DataSO/Date/Default", order = 0)]
public class DateSO : ScriptableObject
{
    public List<CardOdds> Odds = new List<CardOdds>();
    [Serializable]
    public struct CardOdds
    {
        public float normal;
        public float rare;
        public float epic;
        public float Legend;
    }
}
