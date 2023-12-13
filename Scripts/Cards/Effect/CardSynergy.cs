using System;

[Serializable]
public class CardSynergy
{
    public SynergySO synergy;
    public int value;

    public void ApplySynergy(CageCard cageCard)
    {
        synergy.Apply(cageCard, value);
    }
}