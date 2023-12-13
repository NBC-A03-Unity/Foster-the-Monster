using UnityEngine;
using static Enums;

public class UIFosMonInfo : UIToolTip
{
    [SerializeField] private UIBar[] uibars = new UIBarIcon[3];

    public void UpdateBar(FosMonIcon iconType, float value, float max)
    {
        uibars[(int)iconType].UpdateBar(value, max);
    }

}
