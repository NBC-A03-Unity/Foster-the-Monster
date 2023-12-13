using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Enums;

public class MonsterSpawner : Singleton<MonsterSpawner>
{
    private ObjectManager objectManager;

    private void Start()
    {
        objectManager = ObjectManager.Instance;
        Init();
    }

    [Header("Monster SpawnSettings")]
    [SerializeField] private MonsterPlanet planet;
    [SerializeField, Range(1, 20)] private int EliteMonsterSpawnChance = 15;
    [SerializeField, Range(1, 10)] private int BossMonsterSpawnChange = 5;

    [Header("For Check")]
    [SerializeField] public List<Transform> spawnPositions;
    [SerializeField] private List<AssetReference> CurPlanetMonsterReferenceList;
    [SerializeField] private List<AssetReference> NormalMonsterList = new List<AssetReference>();
    [SerializeField] private List<AssetReference> EliteMonsterList = new List<AssetReference>();
    [SerializeField] private List<AssetReference> MiniBossMonsterList = new List<AssetReference>();

    [SerializeField] private AssetReference[] PlanetMonsterPrefabLists;

    [SerializeField] private Transform forSetting;
    public async void Init()
    {
        AssetReference PlanetAR;
        AssetReferenceList MonsterAR_List;

        NormalMonsterList.Clear();
        EliteMonsterList.Clear();
        MiniBossMonsterList.Clear();

        if (PlanetMonsterPrefabLists == null) { return; }
        PlanetAR = PlanetMonsterPrefabLists[(int)planet];

        MonsterAR_List = await ResourceManager.Instance.LoadResource<AssetReferenceList>(PlanetAR);
        CurPlanetMonsterReferenceList = MonsterAR_List.list;
        foreach (var assetReference in CurPlanetMonsterReferenceList)
        {
            var tempGO = await objectManager.UsePool<Monster>(assetReference, forSetting);
            switch (tempGO.monsterSO.rarity)
            {
                case MonsterRarity.Basic:
                    NormalMonsterList.Add(assetReference); break;
                case MonsterRarity.Elite:
                    EliteMonsterList.Add(assetReference); break;
                case MonsterRarity.MiniBoss:
                    MiniBossMonsterList.Add(assetReference); break;
                default: break;
            }
            objectManager.ReturnPool(tempGO);
        }
    }

    public void SummonMonster(List<Transform> spawnpositionList, int checkroomCount)
    {
        int random = Random.Range(1, 101);
        MonsterRarity randomRarity;
        if (random <= BossMonsterSpawnChange) { randomRarity = MonsterRarity.MiniBoss; }
        else if (random <= BossMonsterSpawnChange + EliteMonsterSpawnChance) { randomRarity = MonsterRarity.Elite; }
        else { randomRarity = MonsterRarity.Basic; }
        SummonMonster(spawnpositionList, randomRarity);
    }


    public async void SummonMonster(List<Transform> spawnpositionList, MonsterRarity monsterRarity)
    {
        if (spawnpositionList == null || spawnpositionList.Count == 0) return;
        int spawnPositionIndex = Random.Range(0, spawnpositionList.Count);
        int spawnMonsterIndex;
        Vector3 spawnPosition = spawnpositionList[spawnPositionIndex].position;
        switch (monsterRarity)
        {
            case MonsterRarity.Basic:
                spawnMonsterIndex = Random.Range(0, NormalMonsterList.Count);
                await objectManager.UsePool<Monster>(NormalMonsterList[spawnMonsterIndex], spawnPosition, this.transform);
                break;
            case MonsterRarity.Elite:
                spawnMonsterIndex = Random.Range(0, EliteMonsterList.Count);
                await objectManager.UsePool<Monster>(EliteMonsterList[spawnMonsterIndex], spawnPosition, this.transform);
                break;
            case MonsterRarity.MiniBoss:
                spawnMonsterIndex = Random.Range(0, MiniBossMonsterList.Count);
                await objectManager.UsePool<Monster>(MiniBossMonsterList[spawnMonsterIndex], spawnPosition, this.transform);
                break;
            default: break;
        }
    }
}