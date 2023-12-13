using UnityEngine;
using static Enums;

public class InitCatchScene_Frozen : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMClips.CryogeniaBGM);
    }
}
