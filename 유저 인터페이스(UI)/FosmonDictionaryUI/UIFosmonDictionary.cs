using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static Enums;

public class UIFosmonDictionary : MonoBehaviour
{
    [SerializeField] private GameObject monsterButtonPrefab;
    [SerializeField] private Transform monsterListContainer;
    [SerializeField] private AssetReference monsterSOAsset;
    [SerializeField] private Button returnBtn;
    [SerializeField] private GameObject uiLoading;

    private FosmonInfoPanel fosmonInfoScript;

    private void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        returnBtn.onClick.AddListener(OnReturn);
    }

    public async void LoadMonsterForPlanet(MonsterPlanet planet)
    {
        ShowLoadingUI(true);
        ClearMonsterList();
        AssetReferenceList list = await ResourceManager.Instance.LoadResource<AssetReferenceList>(monsterSOAsset);

        int requiredButtons = 0;

        foreach (AssetReference monsterAsset in list.list)
        {
            MonsterSO monster = await ResourceManager.Instance.LoadResource<MonsterSO>(monsterAsset);
            if (monster.planet == planet)
            {
                FosmonButton monsterButton;
                if (requiredButtons < monsterListContainer.childCount)
                {
                    Transform existingButton = monsterListContainer.GetChild(requiredButtons);
                    monsterButton = existingButton.GetComponent<FosmonButton>();
                    existingButton.gameObject.SetActive(true);
                }
                else
                {
                    monsterButton = await ObjectManager.Instance.UsePool<FosmonButton>("FosmonButton", monsterListContainer);
                }
                monsterButton.Initialize(monster, this);
                requiredButtons++;
            }
        }

        for (int i = requiredButtons; i < monsterListContainer.childCount; i++)
        {
            monsterListContainer.GetChild(i).gameObject.SetActive(false);
        }
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Check);
        ShowLoadingUI(false);
    }

    private void ShowLoadingUI(bool show)
    {
        uiLoading.SetActive(show);
    }

    public void ShowMonsterInfo(MonsterSO monster)
    {
        fosmonInfoScript = UIManager.Instance.OpenUI<FosmonInfoPanel>();
        fosmonInfoScript.SetMonsterInfo(monster);
    }

    private void ClearMonsterList()
    {
        foreach (Transform child in monsterListContainer)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnReturn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        UIManager.Instance.CloseUI<UIFosmonDictionary>();
    }
}