using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIPlanetPyroclastia : MonoBehaviour
{
    [SerializeField] private Button returnBtn;
    [SerializeField] private Button moveBtn;
    [SerializeField] private Button leftBtn;

    void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        returnBtn.onClick.AddListener(OnReturnBtn);
        moveBtn.onClick.AddListener(OnMoveBtn);
        leftBtn.onClick.AddListener(OnLeftBtn);
    }

    private void OnLeftBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        UIManager.Instance.CloseUI<UIPlanetPyroclastia>();
        UIManager.Instance.OpenUI<UIPlanetCryogenia>();
    }

    private void OnReturnBtn()
    {
        DataManager.Instance.todayVisitPlanet = false;
        UIManager.Instance.CloseUI<UIPlanetPyroclastia>();
        UIManager.Instance.OpenUI<UIMainRoom>();
    }

    private void OnMoveBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        LoadingSceneController.LoadScene(SceneName.CatchScene_Lava);
    }
}
