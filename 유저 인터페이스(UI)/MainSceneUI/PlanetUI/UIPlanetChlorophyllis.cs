using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIPlanetChlorophyllis : MonoBehaviour
{
    [SerializeField] private Button returnBtn;
    [SerializeField] private Button moveBtn;
    [SerializeField] private Button rightBtn;

    void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        returnBtn.onClick.AddListener(OnReturnBtn);
        moveBtn.onClick.AddListener(OnMoveBtn);
        rightBtn.onClick.AddListener(OnRightBtn);
    }

    private void OnRightBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        UIManager.Instance.CloseUI<UIPlanetChlorophyllis>();
        UIManager.Instance.OpenUI<UIPlanetCryogenia>();
    }

    private void OnReturnBtn()
    {
        DataManager.Instance.todayVisitPlanet = false;
        UIManager.Instance.CloseUI<UIPlanetChlorophyllis>();
        UIManager.Instance.OpenUI<UIMainRoom>();
    }

    private void OnMoveBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        LoadingSceneController.LoadScene(SceneName.CatchScene);
    }
}
