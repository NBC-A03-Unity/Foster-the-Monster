using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utility;

public class UICage : MonoBehaviour
{
    private const float DURATION = 0.3f;
    private const string redColor = "FE6461";
    private const string yellowColor = "FEFA6D";
    private const int E_NOT_ENOUGH_MONETY= 4001;
    private const int K_NOT_ENOUGH_MONETY= 3001;

    [Header("UI")]
    [SerializeField] public Transform cageCameraUI;

    [Header("Button")]
    [SerializeField] private Button addCageBtn;
    [SerializeField] private Button resetCamBtn;

    [Header("Text")]
    [SerializeField] private TMP_Text addCageBtnText;
    [SerializeField] private TMP_Text resetCamBtnText;

    [SerializeField] private TMP_Text goldTxt;

    private void Awake()
    {
        addCageBtn.onClick.AddListener(OnAddCageBtn);
        resetCamBtn.onClick.AddListener(OnResetCamBtn);
        goldTxt.text = DataManager.Instance.cagePrice.ToString();
    }
    private void Start()
    {
        InitializeButtonHoverEffects();
    }
    private void InitializeButtonHoverEffects()
    {
        UIManager.Instance.InitializeButtonHoverEffect(addCageBtn, addCageBtnText, yellowColor);
        UIManager.Instance.InitializeButtonHoverEffect(resetCamBtn, resetCamBtnText, redColor);
        
    }
    private void OnAddCageBtn()
    {
        if (!DataManager.Instance.MainSceneTutorial) return;
        if (DataManager.Instance.Player.Gold < DataManager.Instance.cagePrice)
        {
            CheckPopUp(E_NOT_ENOUGH_MONETY, K_NOT_ENOUGH_MONETY);
        }
        else
        {
            CheckAddCagePopUp();
        }
        
    }
    private void OnResetCamBtn()
    {
        if (!DataManager.Instance.MainSceneTutorial) return;
        Camera.main.transform.DOMove(new Vector3(0, 0, -10), DURATION);
        CardManager.Instance.CloseUIHand();
        CageManager.Instance.CurrentCage = null;
        UIManager.Instance.CloseUI<UICageInfo>();
        UIManager.Instance.CloseUI<UIFosMonInfo>();
        UIManager.Instance.CloseUI<UICageItem>();
        UIManager.Instance.CloseUI<UICageCard>();
        UIManager.Instance.OpenUI<UIMainRoom>();

    }
    private void CheckAddCagePopUp()
    {
        int exitPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2003 : 1003;

        UIManager.Instance.OpenConfirmationPopup(
            exitPopupKey,
            () =>
            {
                UIManager.Instance.CloseUI<ConfirmationPopup>();
                DataManager.Instance.Player.ChangeGold(-DataManager.Instance.cagePrice);
                DataManager.Instance.cagePrice *= 2;
                goldTxt.text = DataManager.Instance.cagePrice.ToString();
                CageManager.Instance.AddCage();
            },
            () =>
            {
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );
    }
}