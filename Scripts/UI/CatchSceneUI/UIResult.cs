using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIResult : MonoBehaviour
{
    public int count;
    PlayerStatus status;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _roomText;
    [SerializeField] private TextMeshProUGUI _monsterText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _countMaxText;
    [Header("Button")]
    [SerializeField] private Button exitBtn;
    [SerializeField] Transform UISelectMonster;
    private List<SelectMonsterBtn> UISelectMonsterBtns;

    private void Awake()
    {
        UISelectMonsterBtns = new List<SelectMonsterBtn>();
        InitializeButtonListeners();

        foreach (SelectMonsterBtn btn in UISelectMonster.GetComponentsInChildren<SelectMonsterBtn>())
        {
            btn.ConnectUIResult(this);
            UISelectMonsterBtns.Add(btn);
        }
    }

    private void OnEnable()
    {
        if (status != null)
        {
            UpdateUIText();
            UpdateUISelectMonster();
        }
        
    }
    private void InitializeButtonListeners()
    {
        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(OnExitBtn);
    }

    private void OnExitBtn()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Warp);
        LoadingSceneController.LoadScene(SceneName.MainScene);
    }
    private string DropGold()
    {
        int gold = 0;
        foreach(MonsterData md in DataManager.Instance.CaughtMonsters)
        {
            gold += md._data.DropGold;
        }
        DataManager.Instance.Player.ChangeGold(gold);

        return gold.ToString();
    }
    public void UpdateUIText()
    {
        int hours = ((int)status._time / 3600);
        int minutes = ((int)status._time % 3600 / 60);
        int seconds = ((int)status._time % 60);

        _timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        _roomText.text = status._checkedRoom.ToString();
        _monsterText.text = DataManager.Instance.CaughtMonsters.Count.ToString();

        _goldText.text = DropGold();
        UpdateSelectCount();
    }

    public void UpdateSelectCount()
    {
        int maxCount = (DataManager.Instance.DayCaughtMonsterCount < DataManager.Instance.emptyCageCount ?
            DataManager.Instance.DayCaughtMonsterCount : DataManager.Instance.emptyCageCount);

        _countMaxText.text = maxCount.ToString();
        _countText.text = count.ToString();
    }

    public void ConnectPlayerStatus(PlayerStatus status)
    {
        this.status = status;
        UIManager.Instance.CloseUI<UIResult>();
    }

    public void UpdateUISelectMonster()
    {
        count = 0;
        for (int i = 0; i < UISelectMonsterBtns.Count; i++)
        {
            if (DataManager.Instance.CaughtMonsters.Count > 0)
            {
                UISelectMonsterBtns[i].Init(DataManager.Instance.CaughtMonsters[0]);
                DataManager.Instance.CaughtMonsters.RemoveAt(0);
                UISelectMonsterBtns[i].OnUI();
                UISelectMonsterBtns[i].OnBtn();
            }
            else
            {
                UISelectMonsterBtns[i].OffUI();
            }
        }

        DataManager.Instance.CaughtMonsters.Clear();
    }

    public void AllOffBtn()
    {
        foreach(SelectMonsterBtn btn in UISelectMonsterBtns)
        {
            btn.OffBtn();
        }
    }

}
