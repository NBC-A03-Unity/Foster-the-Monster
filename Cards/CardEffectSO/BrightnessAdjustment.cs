using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "DataSO/CardEffect/Brightness", order = 1)]
public class BrightnessAdjustment : Effect
{
    public override void ApplyEffect(CageMainController cage, int value)
    {
        cage.model.Brightness += value;
    }
}