public class MainSceneGameManager: Singleton<MainSceneGameManager>
{
    public UIMainScene UIMainScene { get; private set; }
    public UIMainRoom UIMainRoom { get; private set; }
    public UICage UICage { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.Init();
        AudioManager.Instance.PlayBGM(Enums.BGMClips.MainSceneBGM);
        UIMainScene = UIManager.Instance.OpenUI<UIMainScene>();
        UICage = UIManager.Instance.OpenUI<UICage>();
        UIMainRoom = UIManager.Instance.OpenUI<UIMainRoom>();
    }
}