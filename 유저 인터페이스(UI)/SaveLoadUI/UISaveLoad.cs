using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UISaveLoad : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text caughtMonsterCountText;
    [SerializeField] private TMP_Text goldText;

    [Header("Button")]
    [SerializeField] private Button loadBtn;
    [SerializeField] private Button closeBtn;

    private void Start()
    {
        InitializeButtonListeners();
        UpdateSlotDisplay();
    }

    private void InitializeButtonListeners()
    {
        closeBtn.onClick.AddListener(OnCloseBtn);
        loadBtn.onClick.AddListener(Load);
    }

    private void Load()
    {
        Time.timeScale = 1f;
        DataManager.Instance.LoadDataManager(SaveLoadManager.Instance.LoadData());
        AudioManager.Instance.StopBGM();
        
        LoadingSceneController.LoadScene(SceneName.MainScene);

    }

    private void OnCloseBtn()
    {
        UIManager.Instance.CloseUI<UISaveLoad>();
    }

    private void UpdateSlotDisplay()
    {
        DataManager data = SaveLoadManager.Instance.LoadData();
        if (data != null)
        {
            dateText.text = data.date.ToString();
            caughtMonsterCountText.text = data.cageCount.ToString();
            goldText.text = data.Player.Gold.ToString();
    }
        else
        {
            dateText.text = "Empty";
            caughtMonsterCountText.text = "Empty";
            goldText.text = "Empty";
        }
    }

}
