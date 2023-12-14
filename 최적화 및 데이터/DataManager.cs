using System;
using System.Collections.Generic;
using static Enums;

[Serializable]
public class DataManager : NonMonoSingleton<DataManager>
{
    public void LoadDataManager(DataManager dm)
    {
        DayCaughtMonsterCount = dm.DayCaughtMonsterCount;
        date = dm.date;
        cageCount = dm.cageCount;
        cagePrice = dm.cagePrice;
        emptyCageCount = dm.emptyCageCount;
        resultCount = dm.resultCount;
        selectCount = dm.selectCount;
        selectCardOptions = dm.selectCardOptions;
        drawCardCount = dm.drawCardCount;
        RequestMonsterTypes = dm.RequestMonsterTypes;
        CompleteIDs = dm.CompleteIDs;
        todayCardSelect = dm.todayCardSelect;
        todayVisitPlanet = dm.todayVisitPlanet;
        GameOver = dm.GameOver;
        SelectCardWeightPool = dm.SelectCardWeightPool;
        weight = dm.weight; 

        Cages = dm.Cages;
        CardContainer = dm.CardContainer;
        SelectMonsters = dm.SelectMonsters;
        CaughtMonsters = dm.CaughtMonsters;
        EscapeMonsters = dm.EscapeMonsters;
        caughtMonsterIds = dm.caughtMonsterIds;

        eventData = dm.eventData;

        MainSceneTutorial = dm.MainSceneTutorial;
        Day1CutScene = dm.Day1CutScene;

        Player = dm.Player;
        energyPrice = dm.energyPrice;
        jumpPrice = dm.jumpPrice;
        speedPrice = dm.speedPrice;
        attackPrice = dm.attackPrice;
        attackSpeedPrice = dm.attackSpeedPrice;

    }
    public int date;
    public string currentScene;
    public int cageCount;
    public int cagePrice;
    public int resultCount;
    public int selectCardOptions;
    public int drawCardCount;
    public int selectCount;

    public int RequestMonsterTypes;
    public HashSet<int> CompleteIDs;

    public bool todayCardSelect;
    public bool todayVisitPlanet;
    public bool GameOver;

    public int energyPrice;
    public int jumpPrice;
    public int speedPrice;
    public int attackPrice;
    public int attackSpeedPrice;

    public int energyStat;
    public int jumpStat;
    public float speedStat;
    public float attackStat;
    public float attackSpeedStat;

    public bool MainSceneTutorial;
    public bool Day1CutScene;

    public EventData eventData;
    public List<Cage> Cages { get; private set; }
    public int emptyCageCount;
    public CardContainer CardContainer { get; private set; }

    public Dictionary<int, int> SelectCardWeightPool { get; private set; }

    public int weight;
    public List<MonsterData> CaughtMonsters { get; private set; }
    public List<MonsterData> SelectMonsters { get; set; }
    public List<MonsterData> EscapeMonsters { get; private set; }

    public HashSet<int> caughtMonsterIds = new HashSet<int>();
    public Player Player { get; private set; }

    public int DayCaughtMonsterCount;

    public void CatchMosnter(MonsterSO monster)
    {
        caughtMonsterIds.Add(monster.monsterID);
    }

    public bool HasCaughtMonster(int monsterID)
    {
        return caughtMonsterIds.Contains(monsterID);
    }
    public void EndDayComplete()
    {
        selectCount+=2;
        todayCardSelect = false;
        CardManager.Instance.CardSelectController.AddCardPool();
    }

    public void ReportEvent()
    {
        if (date >= 1 && !Day1CutScene)
        {
            Day1CutScene = true;
            LoadingSceneController.LoadScene(SceneName.MovieScene_Report);
        }
    }
    public void SavePlayerData(Player player)
    {
        Player = player;
    }

    public DataManager()
    {
        ResetDataForNewGame();
    }

    public void SaveData()
    {
        if (resultCount == 0)
        {
            SavePopUp();
            
        }
    }

    public void UpdatePlayerStat()
    {
        Player = Player.SetTenacity((Player.MaxTenacity<energyStat)?energyStat:Player.MaxTenacity).
            SetJump(Player.MaxJump<jumpStat?jumpStat:Player.MaxJump).
            SetSpeed(Player.MoveSpeed<speedStat?speedStat:Player.MoveSpeed).
            SetAttack(Player.Attack<attackStat?attackStat:Player.Attack).
            SetAttackSpeed(Player.AttackSpeed<attackSpeedStat?attackSpeedStat:Player.AttackSpeed);
    }
    public void SavePopUp()
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? 4002 : 3002;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                SaveLoadManager.Instance.SaveData(this);
                ReportEvent();
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
            }
        );

    }

    public void ResetDataForNewGame()
    {
        DayCaughtMonsterCount = 0;
        date = 0;
        cageCount = 0;
        cagePrice = 50;
        emptyCageCount = 0;
        resultCount = 0;
        selectCount = 2;
        selectCardOptions = 3;
        drawCardCount = 5;
        RequestMonsterTypes = 1;
        CompleteIDs = new HashSet<int>();
        todayCardSelect = false;
        todayVisitPlanet = false;
        GameOver = false;
        energyPrice = 50;
        jumpPrice = 150;
        speedPrice = 50;
        attackPrice = 50;
        attackSpeedPrice = 50;
        energyStat = 100;
        jumpStat = 2;
        speedStat = 5f;
        attackStat = 0f;
        attackSpeedStat = 0f;
        SelectCardWeightPool = new Dictionary<int, int>();
        weight = 0;
         Cages = new List<Cage>();
        CardContainer = new CardContainer();

        eventData = new EventData();

        MainSceneTutorial = false;
        Day1CutScene = false;

        SelectMonsters = new List<MonsterData>();
        CaughtMonsters = new List<MonsterData>();
        EscapeMonsters = new List<MonsterData>();
        caughtMonsterIds = new HashSet<int>();

        Player = new Player(100, 100, 50, 2, WeaponType.None, 5, 0);
    }
}
