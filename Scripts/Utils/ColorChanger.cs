using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static Enums;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Color hoverColor = new Color(0.596f, 0.969f, 0.776f, 1f);

    private Dictionary<Graphic, Color> originalColors = new Dictionary<Graphic, Color>();

    public void AddHoverEffect(Graphic uiElement)
    {
        if (!originalColors.ContainsKey(uiElement))
        {
            originalColors[uiElement] = uiElement.color;
        }

        var eventTrigger = uiElement.gameObject.GetComponent<EventTrigger>() ?? uiElement.gameObject.AddComponent<EventTrigger>();
        AddEventTrigger(eventTrigger, EventTriggerType.PointerEnter, (data) => { OnHoverEnter(uiElement); });
        AddEventTrigger(eventTrigger, EventTriggerType.PointerExit, (data) => { OnHoverExit(uiElement); });
    }

    private void OnHoverEnter(Graphic uiElement)
    {
        uiElement.color = hoverColor;
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Hover);
    }

    private void OnHoverExit(Graphic uiElement)
    {
        uiElement.color = originalColors[uiElement];
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}
