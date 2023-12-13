using UnityEngine;
using UnityEngine.UI;

public class SelectMonsterBtn : MonoBehaviour
{
    [SerializeField] GameObject check;
    [SerializeField] Button btn;
    [SerializeField] Image image;
    private MonsterData data = null;
    private UIResult result;

    public void ConnectUIResult(UIResult result)
    {
        this.result = result;
    }
    public void Init(MonsterData data)
    {
        this.data = data;  
        image.sprite = data._data.mosterSprite;
        btn.onClick.AddListener(OnBtn);
    }

    public void OnUI()
    {
        gameObject.SetActive(true);
    }

    public void OffUI()
    {
        gameObject.SetActive(false);
    }

    public void OnBtn()
    {
        if (data == null) return;
        if (result.count < DataManager.Instance.DayCaughtMonsterCount && result.count < DataManager.Instance.emptyCageCount)
        {
            result.count++;
            check.SetActive(true);
            DataManager.Instance.SelectMonsters.Add(data);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OffBtn);
            result.UpdateSelectCount();
        }
        
    }

    public void OffBtn()
    {
        if (data == null) return;
        check.SetActive(false);


        if (DataManager.Instance.SelectMonsters.Contains(data))
        {
            DataManager.Instance.SelectMonsters.Remove(data);
            result.count--;
        }
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnBtn);
        result.UpdateSelectCount();
    }
}
