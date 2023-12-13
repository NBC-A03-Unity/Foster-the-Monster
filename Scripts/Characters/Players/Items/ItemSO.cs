using System.Collections.Generic;
using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "ItemData", menuName = "DataSO/Item/Default", order = 0)]
public class ItemSO : ScriptableObject
{
    [Header("Default")]
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemSprite;
    public ItemType ItemType;
    public WeaponType WeaponType;
    public CardRarity Rarity;
    public float ItemParameter;
    public int ItemID;
    public List<StimulusData> stimulusDatas;
}
