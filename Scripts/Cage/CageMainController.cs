using System;
using UnityEngine;
using static Enums;

public class CageMainController : MonoBehaviour
{
    public Cage model;
    public CageView view;

    public CageInfoController cageInfoController;
    public FosMonInfoController fosMonInfoController;
    public CageItemController cageItemController;
    public CageCardController cageCardController;   

    private Monster CurrentMonster;

    private void OnDestroy()
    {
        ObjectManager.Instance.ReturnPool(CurrentMonster);
    }

    public void InitNewCage(RenderTexture renderTexture, CageBtn cageBtn)
    {
        view = GetComponent<CageView>();
        view.cageBtn = cageBtn;
        view.mCamera.gameObject.SetActive(true);
        view.mCamera.targetTexture = renderTexture;
        view.cageBtn.InitBtn(renderTexture, () => view.MoveCameraToCage(this));
        model = new Cage();
        cageInfoController = new CageInfoController(this);
        fosMonInfoController = new FosMonInfoController(this);
        cageItemController = new CageItemController(this);
        cageCardController = new CageCardController(this);
        
    }
    public void InitLoadCage(RenderTexture renderTexture, CageBtn cageBtn, Cage cage)
    {
        view = GetComponent<CageView>();
        view.cageBtn = cageBtn;
        view.mCamera.gameObject.SetActive(true);
        view.mCamera.targetTexture = renderTexture;
        view.cageBtn.InitBtn(renderTexture, () => view.MoveCameraToCage(this));
        model = cage;
        cageInfoController = new CageInfoController(this);
        fosMonInfoController = new FosMonInfoController(this);
        cageItemController = new CageItemController(this);
        cageCardController = new CageCardController(this);
        if (model.LoadCageData())
        {
            LoadMonster(model.monster);
        }
        else
        {
            CurrentMonster = null;
            view.OffWaringUI();
        }
    }
    public void SelectThisCage()
    {
        cageInfoController.SelectThisCage();
        fosMonInfoController.SelectThisCage();
        cageItemController.SelectThisCage();
        cageCardController.SelectThisCage(UpdateMonster);
    }
    public void UpdateCage()
    {
        if (model.monster == null || model.monster._data == null) return;
        cageCardController.CheckCardSuccess();
        DataManager.Instance.resultCount++;
        model.isUse = true;

    }
    public void UpdateMonster()
    {
        DataManager.Instance.resultCount--;

        cageCardController.Usecard();
        model.monster.UpdateMonster();
        fosMonInfoController.UpdateViewFosMonInfo();

        if (model.monster.CurAchievement >= 100)
        {
            AchievementPopUp(TrySave);
        }
        else if(model.monster.CurStress >= 100)
        {
            EscapeMonsterPopUp(TrySave);
        }
        else
        {
            TrySave();
        }
    }
    public bool AddCard(CardSO so)
    {
        bool result;
        switch (so.cardType)
        {
            case (CardType.Item):
                result = model.AddItem(so.cardId);
                if (result) UseCardAsync(so.cardId);
                cageItemController.UpdateViewCageItem();
                break;
            default:
                result = cageCardController.CheckCard(so);
                break;
        }
        cageCardController.UpdateCardInfo();
        return result;
    }
    public void AddMonster(MonsterData monsterData)
    {
        model.monster = monsterData;
        LoadMonster(monsterData);
       
    }
    
    private async void UseCardAsync(int id)
    {
        CardSO so = await ResourceManager.Instance.LoadResource<CardSO>($"CardSO_{id}");
        so.UseCard(this);
    }
    private async void LoadMonster(MonsterData monsterData)
    {
        CurrentMonster = await ObjectManager.Instance.UsePool<Monster>($"MonsterPrefab_{monsterData.MonsterID}", view.MonsterPosition);
        CurrentMonster.transform.localPosition = Vector3.zero;

        view.AddMonsterButton(() =>
        {
            UIManager.Instance
            .OpenUI<FosmonInfoPanel>()
            .SetMonsterInfo(monsterData._data);
        });
    }
    private void TrySave()
    {
        if (DataManager.Instance.resultCount == 0)
        {
            DataManager.Instance.SaveData();
            CardManager.Instance.LoadOrDrawCard();
        }
        else
        {
            CardManager.Instance.CheckResult();
        }
    }
    private void RemoveMonster()
    {
        ObjectManager.Instance.ReturnPool(CurrentMonster);
        DataManager.Instance.emptyCageCount++;
        CurrentMonster = null;
        model.monster = null;
        view.OffWaringUI();
        UIManager.Instance.CloseUI<UIFosMonInfo>();
    }
    private void AchievementPopUp(Action action)
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? 4005 : 3005;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                DataManager.Instance.CompleteIDs.Add(model.monster._data.monsterID);
                DataManager.Instance.Player.ChangeGold(model.monster._data.ResearchGold);
                RemoveMonster();
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
                action?.Invoke();
            }
        );
    }
    private void EscapeMonsterPopUp(Action action)
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? 4008 : 3008;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                RemoveMonster();
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
                action?.Invoke();
            }
        );
    }
}