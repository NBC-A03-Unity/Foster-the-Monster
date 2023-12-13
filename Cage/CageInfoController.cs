using static Enums;

public class CageInfoController
{
    private Cage model;
    private UICageInfo view;
    public CageInfoController(CageMainController cage)
    {
        model = cage.model;
        model.OnTemperatureChange += UpdateTemperatureBarIcon;
        model.OnBrightnessChange += UpdateBrightnessBarIcon;
        model.OnCleanlinessChange += UpdateCleanlinessBarIcon;
    }
    public void SelectThisCage()
    {
        UIManager.Instance.CloseUI<UICageInfo>();
        view = UIManager.Instance.OpenUI<UICageInfo>();
        UpdateTemperatureBarIcon();
        UpdateBrightnessBarIcon();
        UpdateCleanlinessBarIcon();
    }

    private void UpdateTemperatureBarIcon()
    {
        MonsterSatisfaction satisfaction =
            model.monster == null || model.monster._data == null ?
            MonsterSatisfaction.None :
            model.monster.UpdateMonsterByTemperature(model.Temperature);
        if (view == null) return;
        view.UpdateBarIcon(CageIcon.Temperature, satisfaction, model.Temperature);
    }
    private void UpdateBrightnessBarIcon()
    {
        MonsterSatisfaction satisfaction =
            model.monster == null || model.monster._data == null ?
            MonsterSatisfaction.None :
            model.monster.UpdateMonsterByBrightness(model.Brightness);

        if (view == null) return;
        view.UpdateBarIcon(CageIcon.Brightness, satisfaction, model.Brightness);
    }
    private void UpdateCleanlinessBarIcon()
    {
        MonsterSatisfaction satisfaction =
            model.monster == null || model.monster._data == null ?
            MonsterSatisfaction.None :
            model.monster.UpdateMonsterByCleanliness(model.Cleanliness);
        if (view == null) return;
        view.UpdateBarIcon(CageIcon.Cleanliness, satisfaction, model.Cleanliness);
    }
}