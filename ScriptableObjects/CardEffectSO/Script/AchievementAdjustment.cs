using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "DataSO/CardEffect/Achievement", order = 5)]
public class AchievementAdjustment : Effect
{
    public override void ApplyEffect(CageMainController cage, int value)
    {
        cage.fosMonInfoController.UpdateAchievement(value);
    }
}