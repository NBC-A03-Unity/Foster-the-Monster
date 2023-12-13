using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField] private Image progressBar;
    [SerializeField] private Image progressBarBG;
    [SerializeField] private GameObject helpUI;
    [SerializeField] private TMP_Text spaceBarText;

    public static void LoadScene(SceneName sceneName)
    {
        nextScene = sceneName.ToString();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResetUIManager();
        }

        SceneManager.LoadScene("LoadingScene");
    }


    private void Start()
    {
        AudioManager.Instance.StopBGM();
        OnHelpUI();
        StartCoroutine(LoadSceneProcess());
    }

    private void OnHelpUI()
    {
        if (nextScene == "CatchScene" || nextScene == "CatchScene_Frozen" || nextScene == "CatchScene_Lava")
        {
            helpUI.SetActive(true);
        }
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    SetImageTransparency(progressBar, 0f);
                    SetImageTransparency(progressBarBG, 0f);
                    AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Check);
                    spaceBarText.gameObject.SetActive(true);

                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void SetImageTransparency(Image image, float alpha)
    {
        if(image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
