using UnityEngine;

[System.Serializable]
public struct PopupData
{
    public int popupKey;
    public string title;
    public string message;
    public string leftButtonText;
    public string rightButtonText;
}

[CreateAssetMenu(fileName = "PopupSO", menuName = "Popup/PopupSO", order = 0)]
public class PopupSO : ScriptableObject
{
    public PopupData[] popups;
}
