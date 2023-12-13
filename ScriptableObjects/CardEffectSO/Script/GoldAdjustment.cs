using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "DataSO/CardEffect/Gold", order = 6)]
public class GoldAdjustment : Effect
{
    public override void ApplyEffect(CageMainController cage, int value)
    {
        DataManager.Instance.Player.ChangeGold(value);
    }
}