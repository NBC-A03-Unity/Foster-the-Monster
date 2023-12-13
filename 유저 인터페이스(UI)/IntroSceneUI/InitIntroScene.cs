using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using static Enums;

public class InitIntroScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private GameObject uiIntro;

    private void Start()
    {
        PlayIntro();
        StartCoroutine(PlayBGMAfterDelay(BGMClips.Intro1, 0));
        StartCoroutine(PlayBGMAfterDelay(BGMClips.Intro2, 4f));
    }

    private IEnumerator PlayBGMAfterDelay(BGMClips clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlayBGM(clip);
    }

    private void PlayIntro()
    {
        timelineDirector.Play();
        uiIntro.SetActive(true);

        timelineDirector.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (director == timelineDirector)
        {
            CloseIntro();
            LoadingSceneController.LoadScene(SceneName.StartScene);
        }
    }

    private void CloseIntro()
    {
        timelineDirector.gameObject.SetActive(false);
        uiIntro.SetActive(false);
    }

    private void OnDestroy()
    {
        timelineDirector.stopped -= OnTimelineFinished;
    }
}
