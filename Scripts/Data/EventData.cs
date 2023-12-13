using System;
using System.Collections.Generic;

[Serializable]
public class EventData
{
    public List<int> eventDate;
    public List<int> goldDate;
    public int bossDate;

    public EventData()
    {
        eventDate = new List<int> { 4, 8, 12, 16 };
        goldDate = new List<int> { 5, 10, 15 };
        bossDate = 20;
    }
}