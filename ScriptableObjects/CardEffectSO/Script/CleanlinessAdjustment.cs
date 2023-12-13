using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "DataSO/CardEffect/Cleanliness", order = 0)]
public class CleanlinessAdjustment : Effect
{

    public override void ApplyEffect(CageMainController cage, int value)
    {
        cage.model.Cleanliness += value;
    }
}