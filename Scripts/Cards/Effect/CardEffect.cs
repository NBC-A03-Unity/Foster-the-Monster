using System;
using static Utility;

[Serializable]
public class CardEffect
{
    public Effect effect;
    public int value;

    public void MakeCardEffectTxt(int value)
    {
        sb.Append($"{effect.effectName} ");
        sb.AppendLine(value < 0 ? $"{-value} 감소" : value > 0 ? $"{value} 증가" : "변화 없음");
    }
}