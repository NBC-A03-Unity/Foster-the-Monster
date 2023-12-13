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
        sb.AppendLine(value < 0 ? $"{-value} ����" : value > 0 ? $"{value} ����" : "��ȭ ����");
    }
}