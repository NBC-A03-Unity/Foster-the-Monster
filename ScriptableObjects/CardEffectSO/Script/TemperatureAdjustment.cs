using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "DataSO/CardEffect/Temperature", order = 3)]
public class TemperatureAdjustment : Effect
{
    public override void ApplyEffect(CageMainController cage, int value)
    {
        cage.model.Temperature += value;
    }
}