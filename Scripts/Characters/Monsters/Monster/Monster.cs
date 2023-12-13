using System;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public abstract class Monster : MonoBehaviour, ICatchable
{
    [SerializeField] public MonsterSO monsterSO;

    [Header("Sensor")]
    [SerializeField] protected List<MonsterSensor> sensors;

    [Header("Target")]
    public Transform target;

    [Header("MonsterMode")]
    public MonsterMode monsterMode;    

    #region Components
    protected MonsterMovement monsterMovement;
    protected MonsterAI monsterAI;
    #endregion

    public List<Stimulus> StimulusList;
    [SerializeField] protected Stimulus curStimulus;
    public Stimulus CurStimulus
    {
        get { return curStimulus; }
        set
        {
            if (value != curStimulus)
            {
                curStimulus = value;
                OnCurStimulusChange?.Invoke(value,monsterMode);
            }
        }
    }

    [SerializeField] protected MonsterBehaviourState curState;
    public MonsterBehaviourState CurState
    {
        get { return CurState; }
        set 
        { 
            curState = value;
            OnCurStateChanged?.Invoke(value,target);
        }
    }

    public event Action<Stimulus, MonsterMode> OnCurStimulusChange;
    public event Action<List<Stimulus>> OnStimulusListChanged;
    public event Action<MonsterBehaviourState,Transform> OnCurStateChanged;

    [SerializeField] protected MonsterData _monsterData;
    public MonsterData MonsterData => _monsterData;
    protected Rigidbody2D _rigid;
    protected bool isStimulusListChanged;
    protected bool isFighting;

    protected virtual void Awake()
    {
        //_monsterData = new MonsterData(monsterSO.monsterID);
        StimulusList = new List<Stimulus>();
        _rigid = GetComponent<Rigidbody2D>();
        monsterAI = GetComponent<MonsterAI>();
        monsterMovement = GetComponent<MonsterMovement>();
        MonsterSensor[] newSensors = GetComponentsInChildren<MonsterSensor>();
        foreach (MonsterSensor sensor in newSensors) 
        { 
            sensors.Add(sensor);
        }
    }

    protected virtual void OnEnable()
    {
        _monsterData = new MonsterData(monsterSO.monsterID);
        Init();
    }

    public void Init()
    {
        foreach (MonsterSensor sensor in sensors)
        {
            sensor.Init(monsterSO);
            AddListenerToSensor(sensor);
        }
        monsterMovement.Init(monsterSO);
        monsterMode = MonsterMode.Peace;
        CurState = MonsterBehaviourState.Wandering;
        OnCurStimulusChange?.Invoke(curStimulus, monsterMode);
        if (monsterSO.isFlyable)
        {
            gameObject.layer = LayerMask.NameToLayer("FlyingMonster");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("LandMonster");
        }
    }


    public void AddListenerToSensor(MonsterSensor sensor)
    {
        sensor.OnSense -= AddStimulusList;
        sensor.OnSense += AddStimulusList;
        sensor.UnSense -= RemoveStimulusList;
        sensor.UnSense += RemoveStimulusList;
    }
    public void RemoveListenerToSensor(MonsterSensor sensor)
    {
        sensors.Remove(sensor);
        sensor.OnSense -= AddStimulusList;
        sensor.UnSense -= RemoveStimulusList;
    }

    protected void AddStimulusList(Stimulus stimulus)
    {
        if (stimulus == null || stimulus._data == null) { return; }
        StimulusList.Add(stimulus);
        OnStimulusListChanged?.Invoke(StimulusList);
    }
    protected void RemoveStimulusList(Stimulus stimulus)
    {
        if (stimulus == null || stimulus._data == null) { return; }
        StimulusList.Remove(stimulus);
        if (stimulus == CurStimulus)
        {
            CurStimulus = null;
        }
        OnStimulusListChanged?.Invoke(StimulusList);
    }

    public virtual void BeAttacked(float attack, Transform player, float buff = 0)
    {
        if (monsterMode == MonsterMode.Happy)
        {
            BeCaught();
            return;
        }
        monsterMode = MonsterMode.Rage;
        monsterMovement._animator.SetTrigger("Damage");
        AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.SM_Damage);
        target = player;
        OnCurStimulusChange?.Invoke(curStimulus, monsterMode);

        float random = UnityEngine.Random.Range(0f, 100f);
        float beCaughtChance = 100 - (_monsterData.CurHP - attack)/(_monsterData.CurHP  + attack)*100 + buff;

        if (random < beCaughtChance)
        {
            BeCaught();
        }
        else
        {
            BeDamaged((int)attack);
        }
    }

    public virtual void FallInHappy(Transform newTarget)
    {
        monsterMode = MonsterMode.Happy;
        target = newTarget;
        OnCurStimulusChange?.Invoke(curStimulus, monsterMode);
    }

    public virtual void BeDamaged(int damage)
    {
        _monsterData.DecreaseCurHP(damage);
        
    }

    public virtual void BeCaught()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.SM_Death);
        DataManager.Instance.CaughtMonsters.Add(MonsterData);
        ObjectManager.Instance.ReturnPool(this);
    }
}