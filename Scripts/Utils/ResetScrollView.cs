using UnityEngine;
using UnityEngine.UI;

public class ResetScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;

    private void OnEnable()
    {
        ResetScrollPosition();
    }

    private void ResetScrollPosition()
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
