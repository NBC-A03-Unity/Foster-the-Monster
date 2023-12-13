using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "DataSO/CardEffect/Hunger", order = 2)]
public class HungerAdjustment : Effect
{
    public override void ApplyEffect(CageMainController cage, int value)
    {
        cage.fosMonInfoController.UpdateHunger(value);
    }
}