using UnityEngine;
using static Enums;
public class InitCatchScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMClips.ChlorophyllisBGM);
    }
}