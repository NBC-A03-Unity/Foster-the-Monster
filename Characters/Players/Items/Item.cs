using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO ItemSO;
    public int ItemCount;

    private const string _playerTag = "Player";
    public bool isThrown = false;

    public GameObject stimulusPrefab;
    public List<StimulusData> StimulusDatas;

    [SerializeField] private SpriteRenderer _sr;
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        Init();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(_playerTag)&&!isThrown)
        {
            Player player = DataManager.Instance.Player;

            player.AddToInventory(this, ItemCount);
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        StimulusDatas = ItemSO.stimulusDatas;
        _sr.sprite = ItemSO.ItemSprite;

        if (ItemSO == null || StimulusDatas.Count <= 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < StimulusDatas.Count; i++)
            {
                var newStimulus = Instantiate(stimulusPrefab, transform);
                newStimulus.GetComponent<Stimulus>().Init(StimulusDatas[i]);
            }
        }
    }
}
