using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "DataSO/CardEffect/Stress", order = 4)]
public class StressAdjustment : Effect
{
    public override void ApplyEffect(CageMainController cage, int value)
    {
        cage.fosMonInfoController.UpdateStress(value);
    }

}