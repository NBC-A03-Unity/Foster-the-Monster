using System.Collections.Generic;
using UnityEngine;

public class ItemGameObject : MonoBehaviour
{
    public ItemSO _itemSO;
    public GameObject stimulusPrefab;
    List<StimulusData> datas;

    [SerializeField] private SpriteRenderer _sr;

    public void Init()
    {
        datas = _itemSO.stimulusDatas;
        _sr.sprite = _itemSO.ItemSprite;

        if ( _itemSO == null || datas.Count <= 0)
        {
            return;
        }
        else
        {
            for (int i= 0; i < datas.Count; i++)
            {
                var newStimulus = Instantiate(stimulusPrefab, transform);
                newStimulus.GetComponent<Stimulus>().Init(datas[i]);
            }
        }
    }
}
