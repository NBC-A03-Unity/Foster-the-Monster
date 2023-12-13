using UnityEngine;
using static Enums;

public class InitBossScene : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayBGM(BGMClips.BossBGM);
    }
}