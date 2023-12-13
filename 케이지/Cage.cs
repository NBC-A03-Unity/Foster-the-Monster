using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cage
{
    private int temperature;
    private int brightness;
    private int cleanliness;

    public event Action OnTemperatureChange;
    public event Action OnBrightnessChange;
    public event Action OnCleanlinessChange;
    public int Temperature
    {
        get { return temperature; }
        set
        {
            temperature = value;
            OnTemperatureChange?.Invoke();
        }
    }
    public int Brightness
    {
        get { return brightness; }
        set
        {
            brightness = value;
            OnBrightnessChange?.Invoke();
        }
    }
    public int Cleanliness
    {
        get { return cleanliness;}
        set
        {
            cleanliness = value;
            OnCleanlinessChange?.Invoke();
        }
    }

    public MonsterData monster;
    public int?[] itemIDs;
    public List<int> cardIDs;
    public List<List<int>> cardEffectValues;
    public List<string> cardSuccess;
    public bool isUse = false;
    public Cage()
    {
        temperature = 3;
        brightness = 3;
        cleanliness = 3;
        itemIDs = new int?[3];
        cardIDs = new List<int>();
        cardSuccess = new List<string>();
        cardEffectValues = new List<List<int>>();
    }

    public bool LoadCageData()
    {
        if (monster == null || monster.MonsterID == null) return false;
        monster.LoadSO();
        return true;
    }

    public void CardClear()
    {
        cardIDs.Clear();
        cardSuccess.Clear();
        cardEffectValues.Clear();
    }
    public bool AddItem(int id)
    {
        for (int i = 0; i< itemIDs.Length; i++) 
        {
            if (itemIDs[i] == null)
            {
                itemIDs[i] = id;
                return true;
            }
        }
        return false;
    }
}
