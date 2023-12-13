using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIMainScene : MonoBehaviour
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private TMP_Text goldTxt;
    int currentGold = 0;

    private void Start()
    {
        InitializeButtonListeners();
        UpdateGoldTxt();
        
        
    }

    private void Update()
    {
        if (currentGold != DataManager.Instance.Player.Gold)
        {
            currentGold = DataManager.Instance.Player.Gold;
            UpdateGoldTxt();
        }
    }

    private void InitializeButtonListeners()
    {
        pauseBtn.onClick.AddListener(OnPauseBtn);

    }

    public void OnPauseBtn()
    {
        Time.timeScale = 0;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIPause>();
    }

    public void UpdateGoldTxt()
    {
        goldTxt.text = currentGold .ToString() + " G";
    }
}
