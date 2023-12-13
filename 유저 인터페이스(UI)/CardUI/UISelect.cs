using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static Enums;

public class UISelect : MonoBehaviour
{
    [SerializeField] private TMP_Text cardLoadTxt;
    [SerializeField] private TMP_Text cardcountTxt;
    [SerializeField] public Button reRollBtn;
    [SerializeField] public Transform cardSlot;

    [SerializeField] private AssetReference Card;

    private const float INIT_Y = -800f;
    private const float HIDE_Y = 0f;
    private const float DURATION = 1.3f;

    private bool isLoading = false;
    private const int LOADING = 3;
    private int loadingCount = 0;
    private string InitTxt;

    private void Awake()
    {
        transform.SetParent(CardManager.Instance.CardSelectController.transform);
        InitTxt = cardLoadTxt.text;
    }

    private void OnEnable()
    {
        InitUI();
    }

    private void InitUI()
    {
        DOTween.CompleteAll();
        cardSlot.transform.localPosition = new Vector3(0, HIDE_Y, 0);
        StartLoading();
        cardcountTxt.text = DataManager.Instance.selectCount.ToString();
    }
    public void SettingCompleted()
    {
        DOTween.CompleteAll();
        cardSlot.transform.DOLocalMoveY(INIT_Y, DURATION);
        reRollBtn.onClick.RemoveAllListeners();
        reRollBtn.onClick.AddListener(Reroll);
    }
    public async Task<SelectCard> AddCardObject()
    {
        return await ObjectManager.Instance.UsePool<SelectCard>(Card, cardSlot);

    }
    private void Reroll()
    {
        if (DataManager.Instance.Player.Gold < 5)
        {
            FailReroll();
        }
        else
        {
            CheckReroll();
        }
        
    }
    public void FailReroll()
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? 4004 : 3004;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
            }
        );

    }
    public void CheckReroll()
    {
     
        int exitPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2006 : 1006;

        UIManager.Instance.OpenConfirmationPopup(
            exitPopupKey,
            () =>
            {
                DataManager.Instance.Player.ChangeGold(-5);
                UIManager.Instance.CloseUI<ConfirmationPopup>();
                UIManager.Instance.CloseUI<UISelect>();
                CardManager.Instance.CardSelect();

            },
            () =>
            {
                UIManager.Instance.CloseUI<ConfirmationPopup>();
            }
        );
        
    }
    public IEnumerator Loading()
    {
        while (isLoading)
        {
            cardLoadTxt.text += ".";
            loadingCount++;

            if (loadingCount > LOADING)
            {
                cardLoadTxt.text = InitTxt;
                loadingCount = 0;
            }
            yield return new WaitForSeconds(DURATION / 4);
        }
    }
    public void StartLoading()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Reroll);
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(Loading());
        }
    }

    public void StopLoading()
    {
        isLoading = false;
        UIManager.Instance.CloseUI<UISelect>();
    }

}