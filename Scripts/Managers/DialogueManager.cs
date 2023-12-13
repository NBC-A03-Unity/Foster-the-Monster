using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private DialogueSO[] _koDialogueSO;
    [SerializeField] private DialogueSO[] _engDialogueSO;
    [SerializeField] private float _textTypeDuration = 1.0f;
    private TextMeshProUGUI _dialogueText;

    private List<string> _dialogueList;
    private int _dialogueIndex;
    private int _currentSceneIndex;

    private const string key = "Dialogue";
    private const string langKey = "en-US";


    public void LoadNextDialogue(int sceneIndex)
    {
        if ((_currentSceneIndex != sceneIndex) || _dialogueText == null)
        {
            _dialogueText = GameObject.FindGameObjectWithTag(key).GetComponent<TextMeshProUGUI>();
            _currentSceneIndex = sceneIndex;
            _dialogueIndex = 0;
        }

        _dialogueList = GlobalSettings.CurrentLocale != langKey? _koDialogueSO[sceneIndex].dialogue : _engDialogueSO[sceneIndex].dialogue;
        _dialogueText.text = _dialogueList[_dialogueIndex];
        ShowText(_dialogueText, _textTypeDuration);


        if (_dialogueIndex < _dialogueList.Count - 1)
            _dialogueIndex++;
        else _dialogueIndex = 0;
    }

    private static void ShowText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }

}