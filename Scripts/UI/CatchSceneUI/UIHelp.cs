using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIHelp : MonoBehaviour
{
    [SerializeField] private Button returnBtn;

    private void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        returnBtn.onClick.AddListener(OnReturn);
    }

    private void OnReturn()
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        UIManager.Instance.CloseUI<UIHelp>();
    }
}
