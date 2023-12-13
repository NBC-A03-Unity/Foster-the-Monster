using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "AssetReferenceList", menuName = "AssetReferenceList/Default", order = 0)]

public class AssetReferenceList : ScriptableObject
{
    public List<AssetReference> list = new List<AssetReference>();
}
