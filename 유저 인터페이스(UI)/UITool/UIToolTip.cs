using UnityEngine;
using UnityEngine.EventSystems;

public class UIToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;

    private Canvas parent;
    int order;
    protected virtual void Awake()
    {
        parent = GetComponentInParent<Canvas>();
        order = parent.sortingOrder;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        parent.sortingOrder = UIManager.Instance.CurrentSortingOrder;
        tooltip.SetActive(true);
        Canvas.ForceUpdateCanvases();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DataManager.Instance.MainSceneTutorial)
        {
            parent.sortingOrder = order;
        }
        tooltip.SetActive(false);
    }
}
