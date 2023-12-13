using UnityEngine;
using static Enums;

public class InitStartScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMClips.StartSceneBGM);
        UIManager.Instance.OpenUI<UIStartScene>();
    }
}
