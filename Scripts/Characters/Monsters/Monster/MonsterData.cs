using Newtonsoft.Json;
using System;
using UnityEngine;
using static Enums;

[Serializable]
public class MonsterData
{
    public int? MonsterID;
    public int CurHP;
    public int CurHunger;
    public int CurStress;
    public int CurAchievement;
    public int MaxHP;
    [JsonIgnore] public MonsterSO _data;

    private int temperatureDifference;
    private int cleanlinessDifference;
    private int brightnessDifference;

    public MonsterData()
    {
    }
    public MonsterData(int id)
    {
        InitMonster(id);
    }
    public async void InitMonster(int id)
    {
        _data = await ResourceManager.Instance.LoadResource<MonsterSO>($"MonsterSO_{id}");
        MonsterID = _data.monsterID;
        CurHP = _data.maxHP;
        MaxHP = _data.maxHP;
        CurHunger = _data.maxHunger / 5;
        CurStress = 40;
        CurAchievement = 0;
    }

    public async void LoadSO()
    {
        _data = await ResourceManager.Instance.LoadResource<MonsterSO>($"MonsterSO_{MonsterID}");
    }


    public MonsterSatisfaction UpdateMonsterByCleanliness(int value)
    {
        cleanlinessDifference = Mathf.Abs(value - _data.preferCleanliness);
        return CaculateSatisfactionByDifference(cleanlinessDifference);
    }
    public MonsterSatisfaction UpdateMonsterByBrightness(int value)
    {
        brightnessDifference = Mathf.Abs(value - _data.preferBrightness);
        return CaculateSatisfactionByDifference(brightnessDifference);
    }
    public MonsterSatisfaction UpdateMonsterByTemperature(int value)
    {
        temperatureDifference = Mathf.Abs(value - _data.preferTemperature);
        return CaculateSatisfactionByDifference(temperatureDifference);
    }
    public MonsterSatisfaction CaculateSatisfactionByDifference(int difference)
    {
        MonsterSatisfaction monsterSatisfaction;
        switch (difference)
        {
            case 0:
                monsterSatisfaction = MonsterSatisfaction.Satisfaction; break;
            case 1:
                monsterSatisfaction = MonsterSatisfaction.Average; break;
            default:
                monsterSatisfaction = MonsterSatisfaction.Dissatisfaction; break;
        }

        return monsterSatisfaction;
    }

    public void UpdateMonster()
    {
        CurHunger += _data.dailyHungerIncrease;
        CurHunger = CurHunger < 0 ? 0 : CurHunger > _data.maxHunger ? _data.maxHunger : CurHunger;
        CurStress += CurHunger / (float)_data.maxHunger > 0.5 ? 20 : 10;
        UpdateStress(cleanlinessDifference);
        UpdateStress(temperatureDifference);
        UpdateStress(brightnessDifference);
    }
    public void UpdateStress(int difference)
    {
        int amount = 0;
        switch (difference)
        {
            case 0:
                amount -= 5; break;
            case 1:
                 break;
            default:
                amount += 5; break;
        }
        CurStress += amount;
        CurStress = CurStress < 0 ? 0 : CurStress > 100 ? 100 : CurStress;

    }
    public void IncreaseAchievement(int value)
    {
        CurAchievement += value;
    }
    public void DecreaseCurHP(int value)
    {
        CurHP -= value;
        CurHP = (CurHP<0)? 0:CurHP;
    }
}
