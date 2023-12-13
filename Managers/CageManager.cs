using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Enums;
using static Utility;

public class CageManager : Singleton<CageManager>
{
    private const int MAX_CAGES = 8;
    private const string SOURCE_RENDER_TEXTURE = "CageRenderTexture";
    private const string CAGE_BTN_REF = "CageBtn";
    private const int E_NONE_CAGE = 4006;
    private const int K_NONE_CAGE = 3006;
    private const int E_NONE_MONSTER = 4007;
    private const int K_NONE_MONSTER = 3007;
    private const float DURATION = 0.45f;

    public CageMainController CurrentCage { get; set; }

    [HideInInspector] public List<CageMainController> cageControllers;

    protected override void Awake()
    {
        cageControllers = new List<CageMainController>();
       
    }
    public void Start()
    {
        if (DataManager.Instance.cageCount == 0)
        {
            AddCage(3);
        }
        else
        {
            LoadCage();
        }
    }
    private async void LoadCage()
    {
        List<Cage> cages = DataManager.Instance.Cages;
        for (int i =0; i < cages.Count; i++)
        {
            await CreateLoadCage(cages[i]);
        }
        AddMonsterToCage();
    }
    private void AddMonsterToCage()
    {
        List<MonsterData> monsters = DataManager.Instance.SelectMonsters;

        if (monsters == null) return;

        List<CageMainController> temp = new List<CageMainController>();
        foreach (CageMainController cc in cageControllers)
        {
            if (cc.model.monster == null || cc.model.monster._data == null)
            {
                temp.Add(cc);
                
            }
        }
        while (monsters.Count > 0)
        {
            temp[0].AddMonster(monsters[0]);
            temp.RemoveAt(0);
            monsters.RemoveAt(0);
            DataManager.Instance.emptyCageCount--;
        }

    }

    public void EndDay()
    {
        UpdateCage();
    }

    public void ChangeCurrentCage(CageMainController cmc)
    {
        CurrentCage = cmc;
        cmc.SelectThisCage();
        UIManager.Instance.CloseUI<UIMainRoom>();
        Vector3 pos = new Vector3(cmc.transform.position.x, 0, -10);
        DOTween.Kill(Camera.main);
        Camera.main.transform.DOMove(pos, DURATION).OnComplete(CardManager.Instance.LoadOrDrawCard);
    }

    public void AddCardToCage(CardSO so, int index)
    {
        if (CurrentCage == null)
        {
            CheckPopUp(E_NONE_CAGE, K_NONE_CAGE);
            CardManager.Instance.HandCardController.ResetHand();
        }
        else if (CurrentCage.model.monster == null && so.cardType != CardType.Item)
        {
            CheckPopUp(E_NONE_MONSTER, K_NONE_MONSTER);
            CardManager.Instance.HandCardController.ResetHand();
        }
        else if (CurrentCage.AddCard(so))
        {
            CardManager.Instance.UseCard(index);
        }


    }
    
    public async void AddCage(int count  = 1)
    {
        for (int i = 0; i < count; i++)
        {
            if (DataManager.Instance.cageCount >= MAX_CAGES)
            {
                return;
            }

            DataManager.Instance.Cages.Add(await CreateNewCage());
            DataManager.Instance.cageCount++;
            DataManager.Instance.emptyCageCount++;
        }
        DataManager.Instance.DayCaughtMonsterCount++;
    }

    private async Task<Cage> CreateNewCage()
    {
        CageMainController cage = await ObjectManager.Instance.UsePool<CageMainController>(
                "Cage",
                new Vector3(18 * (cageControllers.Count + 1), 0, 0), transform);
        cageControllers.Add(cage);

        RenderTexture source = await ResourceManager.Instance.LoadResource<RenderTexture>(SOURCE_RENDER_TEXTURE);
        RenderTexture renderTexture = new RenderTexture(source);

        CageBtn cageBtn = await ObjectManager.Instance.UsePool<CageBtn>(CAGE_BTN_REF, MainSceneGameManager.Instance.UICage.cageCameraUI);
        cage.InitNewCage(renderTexture, cageBtn);
        return cage.model;
    }
    private async Task<Cage> CreateLoadCage(Cage model)
    {
        CageMainController cage = await ObjectManager.Instance.UsePool<CageMainController>(
                "Cage",
                new Vector3(18 * (cageControllers.Count + 1), 0, 0), transform);
        cageControllers.Add(cage);

        RenderTexture source = await ResourceManager.Instance.LoadResource<RenderTexture>(SOURCE_RENDER_TEXTURE);
        RenderTexture renderTexture = new RenderTexture(source);

        CageBtn cageBtn = await ObjectManager.Instance.UsePool<CageBtn>(CAGE_BTN_REF, MainSceneGameManager.Instance.UICage.cageCameraUI);
        cage.InitLoadCage(renderTexture, cageBtn, model);
        return cage.model;
    }
    public void UpdateCage()
    {
        foreach(CageMainController c in cageControllers)
        {
            c.UpdateCage();
        }
    }
    public List<int> CageCardList()
    {
        List<int> cardSOID = new List<int>();   
        foreach(CageMainController c in cageControllers)
        {
            foreach (int id in c.model.cardIDs)
            {
                cardSOID.Add(id);
            }
        }
        return cardSOID;
    }



}
