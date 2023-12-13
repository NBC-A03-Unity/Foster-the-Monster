using UnityEngine;
using static Enums;

public class UICageInfo : UIToolTip
{
    private const int MAX_CAGE_INFO_VALUE = 5;
    [SerializeField] private UIBarIcon[] uibars = new UIBarIcon[3];

    public void UpdateBarIcon(CageIcon iconType, MonsterSatisfaction satisfaction, float value)
    {
        uibars[(int)iconType].UpdateBarIcon(satisfaction, value, MAX_CAGE_INFO_VALUE);
    }
    
}
