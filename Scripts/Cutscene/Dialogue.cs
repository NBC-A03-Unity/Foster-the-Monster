using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    private void OnEnable()
    {
        DialogueManager.Instance.LoadNextDialogue(_sceneIndex);
    }
}
