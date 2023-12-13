using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIDeckBtn : MonoBehaviour
{
    private const string mainColor = "98F7C6";
    [SerializeField] private Button deckBtn;
    [SerializeField] private TMP_Text deckBtnHoverText;

    private void Awake()
    {
        deckBtn.onClick.AddListener(OnOpenBtn);
        UIManager.Instance.InitializeButtonHoverEffect(deckBtn, deckBtnHoverText, mainColor);
    }

    public void OnOpenBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        if (!DataManager.Instance.MainSceneTutorial) return;
        if (DataManager.Instance.selectCount > 0)
        {
            CardManager.Instance.CardSelect();
        }
        else
        {
            CardManager.Instance.DeckListController.UpdateDeckUI();
            
        }
    }
}
