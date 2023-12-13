using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIPlanetCryogenia : MonoBehaviour
{
    [SerializeField] private Button returnBtn;
    [SerializeField] private Button moveBtn;
    [SerializeField] private Button rightBtn;
    [SerializeField] private Button leftBtn;

    void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        returnBtn.onClick.AddListener(OnReturnBtn);
        moveBtn.onClick.AddListener(OnMoveBtn);
        rightBtn.onClick.AddListener(OnRightBtn);
        leftBtn.onClick.AddListener(OnLeftBtn);
    }

    private void OnLeftBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        UIManager.Instance.CloseUI<UIPlanetCryogenia>();
        UIManager.Instance.OpenUI<UIPlanetChlorophyllis>();
    }

    private void OnRightBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        UIManager.Instance.CloseUI<UIPlanetCryogenia>();
        UIManager.Instance.OpenUI<UIPlanetPyroclastia>();
    }

    private void OnReturnBtn()
    {
        DataManager.Instance.todayVisitPlanet = false;
        UIManager.Instance.CloseUI<UIPlanetCryogenia>();
        UIManager.Instance.OpenUI<UIMainRoom>();
    }

    private void OnMoveBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        LoadingSceneController.LoadScene(SceneName.CatchScene_Frozen);
    }
}
