using UnityEngine;

public class UIElementContainer
{
    public GameObject GameObject;
    public int SortingOrder;

    public UIElementContainer(GameObject gameobject, int sortingOrder)
    {
        GameObject = gameobject;
        SortingOrder = sortingOrder;
    }
}