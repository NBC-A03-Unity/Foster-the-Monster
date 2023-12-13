using System;
using UnityEngine;
using static Enums;

public class InitTutorialScene : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialText;

    private event Action _onNextTutorial;

    void Start()
    {
        AudioManager.Instance.PlayBGM(BGMClips.MainSceneBGM);
        _tutorialText.gameObject.SetActive(true);


        _onNextTutorial += () =>
        {
            _tutorialText.gameObject.SetActive(false);
            _tutorialText.gameObject.SetActive(true);
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _onNextTutorial.Invoke();
        }
    }
}