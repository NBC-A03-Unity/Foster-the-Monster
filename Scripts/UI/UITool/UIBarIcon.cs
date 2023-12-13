using UnityEngine;
using static Enums;

public class UIBarIcon : UIBar
{
    [SerializeField] private GameObject[] stateIcon;

    public void UpdateBarIcon(MonsterSatisfaction satisfaction, float value, float max)
    {
        UpdateBar(value, max);
        for (int i = 0; i < stateIcon.Length; i++)
        {
            stateIcon[i].SetActive((int)satisfaction == i ? true : false);
        }
    }
}