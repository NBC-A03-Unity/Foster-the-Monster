using UnityEngine;

[System.Serializable]
public struct LocalizedStirng
{
    public int localeKey;
    public string English;
    public string Korean;
}

[CreateAssetMenu(fileName = "FosmonInfoSO", menuName = "UI/FosmonInfoSO", order = 0)]
public class FosmonInfoSO : ScriptableObject
{
    public LocalizedStirng[] localizedStirngs;
}
