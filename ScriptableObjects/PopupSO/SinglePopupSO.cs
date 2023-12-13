using UnityEngine;

[System.Serializable]
public struct SinglePopupData
{
    public int popupKey;
    public string title;
    public string message;
    public string buttonText;
}

[CreateAssetMenu(fileName = "SinglePopupSO", menuName = "Popup/SinglePopupSO", order = 1)]
public class SinglePopupSO : ScriptableObject
{
    public SinglePopupData[] popups;
}
