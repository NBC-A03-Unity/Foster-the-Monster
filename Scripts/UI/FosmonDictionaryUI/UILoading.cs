using DG.Tweening;
using TMPro;
using UnityEngine;

public class UILoading : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingTxt;

    private void Start()
    {
        ShowText(loadingTxt, 4.0f);
    }

    private void ShowText(TMP_Text text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration)
           .SetLoops(-1, LoopType.Restart);
    }
}
