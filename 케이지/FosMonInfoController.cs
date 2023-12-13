using static Enums;

public class FosMonInfoController
{
    private Cage model;
    private UIFosMonInfo view;
    private CageMainController controller;   
    public FosMonInfoController(CageMainController cage)
    {
        controller = cage;
        model = cage.model;
    }

    public void SelectThisCage()
    {
        if (model.monster != null && model.monster._data != null)
        {
            UIManager.Instance.CloseUI<UIFosMonInfo>();
            view = UIManager.Instance.OpenUI<UIFosMonInfo>();
            UpdateViewFosMonInfo();
        }
    }
    public void UpdateViewFosMonInfo()
    {
        UpdateStress(0);
        UpdateHunger(0);
        UpdateAchievement(0);

        if (model.monster.CurStress > 75)
        {
            controller.view.OnWaringUI();
        }
        else
        {
            controller.view.OffWaringUI();
        }
    }
    public void UpdateHunger(int value)
    {
        model.monster.CurHunger += value;
        if (view == null) return;
        view.UpdateBar(FosMonIcon.Hunger, model.monster.CurHunger, model.monster._data.maxHunger);
    }
    public void UpdateStress(int value)
    {
        model.monster.CurStress += value;
        if (view == null) return;
        view.UpdateBar(FosMonIcon.Stress, model.monster.CurStress, 100);
    }
    public void UpdateAchievement(int value)
    {
        model.monster.CurAchievement += value;
        if (view == null) return;
        view.UpdateBar(FosMonIcon.Achievement, model.monster.CurAchievement, 100);
    }
}