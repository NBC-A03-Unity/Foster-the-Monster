using System.Collections.Generic;
using UnityEngine;
using static Enums;
public abstract class MonsterAI : MonoBehaviour
{
    protected Monster monster;
    protected MonsterData monsterData;
    protected MonsterSO monsterSO;
    protected List<Stimulus> stimulusList;

    protected int hostile;
    protected int curious;
    protected int foodloving;
    protected int fearful;


    protected virtual void Awake()
    {
        monster = gameObject.GetComponent<Monster>();
    }
    protected virtual void Start()
    {
        monsterData = monster.MonsterData;
        monsterSO = monster.monsterSO;

        hostile = monsterSO.Hosile;
        curious = monsterSO.Curious;
        foodloving = monsterSO.FoodLoving;
        fearful = monsterSO.Fearful;

        monster.OnStimulusListChanged += ReactStimulusListChanged;
        monster.OnCurStimulusChange += ReactCurStimulusChanged;
    }
    protected virtual void OnDestroy()
    {
        monster.OnStimulusListChanged -= ReactStimulusListChanged;
        monster.OnCurStimulusChange -= ReactCurStimulusChanged;
    }

    protected virtual void ReactStimulusListChanged(List<Stimulus> _stimulusList)
    {
        if (CheckMonsterMode()) { return; }
        int count = _stimulusList.Count;
        int maxLevel = 0;
        Stimulus maxLevelStimulus = null;

        if (count == 0)
        {
            monster.CurStimulus = null;
            return;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                StimulusData data = _stimulusList[i]._data;
                int temperament = 0;
                switch (data.objectType)
                {
                    case StimulusObjectType.Player:
                        temperament = hostile;
                        break;
                    case StimulusObjectType.Object:
                        temperament = curious;
                        break;
                    case StimulusObjectType.Food:
                        temperament = foodloving;
                        break;
                    case StimulusObjectType.Weapon:
                        temperament = fearful;
                        break;
                    default: break;
                }
                if ((monster.monsterSO.preferFoodType == data.foodType) && (data.foodType != FillersFoodType.None))
                {
                    monster.FallInHappy(_stimulusList[i]._data.transform);
                    return;
                }
                if (temperament == 2) 
                {
                    monster.CurStimulus = _stimulusList[i];
                    return;
                }else if(temperament == 1 && maxLevel ==0)
                {
                    maxLevel = 1;
                    maxLevelStimulus = _stimulusList[i];
                }else if(maxLevelStimulus == null)
                {
                    maxLevelStimulus = _stimulusList[i];
                }
            }
            monster.CurStimulus = maxLevelStimulus;
        }
    }
    protected virtual bool CheckMonsterMode()
    {
        if (monster == null) return false;
        if (monster.monsterMode == MonsterMode.Rage)
        {
            monster.CurStimulus = null;
            monster.CurState = MonsterBehaviourState.Rage_Chasing;
            return true;
        }
        if (monster.monsterMode == MonsterMode.Happy)
        {
            monster.CurStimulus = null;
            monster.CurState = MonsterBehaviourState.Happy_Chasing;
            return true;
        }
        return false;
    }

    protected virtual void ReactCurStimulusChanged(Stimulus newStimulus, MonsterMode mode)
    {
        if (CheckMonsterMode()) { return; }
        if (monster == null) return;
        if (newStimulus == null || newStimulus._data == null)
        {
            monster.target = null;
            monster.CurState = MonsterBehaviourState.Wandering;
            return;
        }

        monster.target = newStimulus.gameObject.transform;
        switch (newStimulus._data.objectType)
        {
            case StimulusObjectType.Food:
            case StimulusObjectType.Object:
            case StimulusObjectType.Player:
                monster.CurState = MonsterBehaviourState.Chasing;
                break;
            case StimulusObjectType.Weapon:
                monster.CurState = MonsterBehaviourState.Fleeing;
                break;
            default:
                monster.CurState = MonsterBehaviourState.Wandering;
                break;
        }
    }
}

