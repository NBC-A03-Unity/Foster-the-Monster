using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using static Enums;

public class Director : MonoBehaviour
{
    private PlayableDirector _director;
    [SerializeField] private TimelineAsset[] _timelines;
    [SerializeField] private Button skipButton;
    [SerializeField] private bool _isIntro;
    [SerializeField] private bool _isEndScene;
    [SerializeField] private bool _isBoss;

    private int _index = 0;

    private void Awake()
    {
        AudioManager.Instance.StopBGM();
        Time.timeScale = 1.0f;
        _director = GetComponent<PlayableDirector>();
        LoadNextCut(_director);

        skipButton.onClick.AddListener(SkipToNextCut);
    }

    private void Start()
    {
        _director.stopped += context => LoadNextCut(context);
    }

    private void LoadNextCut(PlayableDirector director)
    {
        if (_index < _timelines.Length)
        {
            director.playableAsset = _timelines[_index];
            _index++;
            director.Play();
        }
        else
        {
            FinishSequence();
        }
    }

    private void SkipToNextCut()
    {
        _director.Pause();

        int skipPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2009 : 1009;

        UIManager.Instance.OpenConfirmationPopup(
            skipPopupKey,
            () => {
                _director.Stop();
                FinishSequence();
            },
            () => {
                AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
                UIManager.Instance.CloseUI<ConfirmationPopup>();
                _director.Resume();
            }
        );
    }

    private void FinishSequence()
    {
        if (_isIntro) LoadingSceneController.LoadScene(SceneName.TutorialScene);
        else if (_isBoss) LoadingSceneController.LoadScene(SceneName.BossScene);
        else if (_isEndScene) LoadingSceneController.LoadScene(SceneName.IntroScene);
        else LoadingSceneController.LoadScene(SceneName.MainScene);
    }
}
