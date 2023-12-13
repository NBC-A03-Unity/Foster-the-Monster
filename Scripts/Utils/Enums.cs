public class Enums
{
    public enum CageIcon
    {
        Temperature,
        Brightness,
        Cleanliness,
    }

    public enum FosMonIcon
    {
        Stress,
        Hunger,
        Achievement,
    }
    public enum WeaponType
    {
        None,
        BasicNet,
        CloseNet,
        Harpoon
    }
    public enum CardRarity
    {
        Normal,
        Rare,
        Epic,
        Legend,
        EndPoint,
    }
    public enum MonsterPlanet 
    { 
        Planet1, 
        Planet2, 
        Planet3, 
        Planet4, 
        Planet5, 
        EndPoint,
    }
    public enum MonsterMode
    {
        Peace,
        Happy,
        Rage,
    }

    public enum CageState
    {
        Default,
        Success,
        Average,
        Fail,
        EndPoint,
    }

    public enum MonsterSatisfaction
    {
        None,
        Satisfaction,
        Average,
        Dissatisfaction,
        EndPoint,
    }
    public enum FillersFoodType
    {
        Meat,
        Grains,
        SeaFood,
        None,
        EndPoint,
    }

    public enum HarvestType
    {
        Feather,
        Horn,
        Iron,
        Scale,
        EndPoint,
    }


    public enum CardType
    {
        FillersFood,
        Fruit,
        Hurb,
        Harvest,
        Item,
        Weapon,
        Experiment,
        EndPoint,
    }

    public enum RoomType
    {
        Spawn,
        Normal,
        Treasure,
        HorizontalCorridor,
        VerticalCorridor,
        Elite
    }

    public enum DoorType
    {
        Left,
        Right,
        Top,
        Bottom
    }
    public enum MonsterBehaviourState 
    { 
        Wandering, 
        Searching, 
        Fleeing, 
        Chasing, 
        Fighting,
        Rage_Chasing,
        Happy_Chasing
    }
    public enum StimulusObjectType 
    { 
        Food, 
        Object, 
        Player, 
        Weapon 
    }

    public enum StimulusType 
    { 
        Odor, 
        Sound, 
        Light 
    }

    public enum PreferPlayerAction 
    { 
        Playing, 
        Disciplining, 
        Washing, 
        Feeding 
    }

    public enum MonsterRarity
    {
        Basic,
        Elite,
        MiniBoss,
        Boss
    }
    public enum ItemType
    {
        Weapon,
        Equipment,
        Item
    }

    public enum MW_AddExternalForceDirection
    {
        Up,
        Down,
        Left,
        Right,
        UpAndDown,
        LeftAndRight,
    }

    public enum SFXCategory
    {
        Button,
        Character,
    }

    public enum SFXClips
    {
        Check,
        Close,
        EndDay,
        Hand,
        Hover,
        Open,
        Pause,
        Reroll,
        Warp,
        BM_Alerted,
        BM_Attack,
        BM_Damage,
        BM_Death,
        Boost,
        Dash,
        Death,
        EnemyHit,
        Hit,
        Jump,
        Run,
        Shoot,
        SM_Alerted,
        SM_Attack,
        SM_Damage,
        SM_Death,
        Swing,
        Tackle
    }

    public enum SceneName
    {
        IntroScene,
        StartScene,
        MainScene,
        LoadingScene,
        CatchScene,
        CatchScene_Lava,
        CatchScene_Frozen,
        BossScene,
        MovieScene,
        MovieScene_Boss,
        MovieScene_Lose,
        MovieScene_Report,
        MovieScene_Win,
        TutorialScene
    }

    public enum BGMClips
    {
        AngryBossBGM,
        BossBGM,
        ChlorophyllisBGM,
        CryogeniaBGM,
        PyroclastiaBGM,
        CutSceneBGM,
        MainSceneBGM,
        StartSceneBGM,
        Intro1,
        Intro2
    }
}
