using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "1_ko_0", menuName = "DataSO/DialogueData", order = 6)]
public class DialogueSO : ScriptableObject
{
    [TextArea]
    public List<string> dialogue;
}
