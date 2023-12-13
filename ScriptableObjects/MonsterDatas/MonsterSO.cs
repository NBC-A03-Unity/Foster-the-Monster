using UnityEngine;
using static Enums;


[CreateAssetMenu(fileName ="New Monster", menuName = "Characters/Monsters/Monster",order = 1)]
public class MonsterSO : ScriptableObject
{
    [Header("FosMon Info")]
    public int monsterID;
    public string monsterName;
    public string description;
    public string monsterKoreanName;
    public string koreanDescription;
    public MonsterPlanet planet;
    public MonsterRarity rarity;

    [Header("Resources")]
    public AudioClip monsterSound;
    public Sprite mosterSprite;

    [Header("FosMon Prefers Setting")]
    [Range (1,5)] public int preferTemperature;
    [Range (1,5)] public int preferCleanliness;
    [Range (1,5)] public int preferBrightness;
    public FillersFoodType preferFoodType;
    public FillersFoodType hateFoodType;
    public HarvestType harvestType;
    public PreferPlayerAction preferPlayerAction;

    [Header("Reward")]
    public int DropGold;
    public int ResearchGold;

    [Header("FosMon Behave Patterns")]
    [Header("Friendly <-> Hostile")]
    [Range(0, 2)] public int Hosile;
    [Header("Indifferent <-> Curious")]
    [Range(0, 2)] public int Curious;
    [Header("Indifferent <-> FoodLoving")]
    [Range(0, 2)] public int FoodLoving;
    [Header("Indifferent <-> Fearful")]
    [Range(0, 2)] public int Fearful;


    [Header("FosMon Status Setting")]
    [Range(1, 10)] public float moveSpeed;
    public float moveAcceleration;
    public float moveForce;
    public bool isFlyable;
    [Range(0, 3)] public float jumpPower;
    public int attack;
    public float attackRange;
    public float attackInterval;
    public int maxHP;
    public int maxHunger;
    [Range(5, 30)]
    public int dailyHungerIncrease;

    [Header("FosMon Sensor Setting")]
    [Range(0, 5)] public int HearingRange;
    public bool HearingSensitive;
    [Range(0, 5)] public int SmellRange;
    public bool SmellSensitive;
    [Range(0, 5)] public int SightRange;
    public bool sightSensitive;

}
