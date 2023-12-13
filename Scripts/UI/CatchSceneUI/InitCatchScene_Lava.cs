using UnityEngine;
using static Enums;

public class InitCatchScene_Lava : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMClips.PyroclastiaBGM);
    }
}
