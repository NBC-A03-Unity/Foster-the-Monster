using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class RoomTemplate : MonoBehaviour
{
    public RoomType Type;
    public GameObject[] Doors;
    private bool _isChecked;
    
    private PlayerStatus _status;

    public float Width;
    public float Height;
    public List<Transform> SpawnPositionList = new List<Transform>();

    private void Awake()
    {
        _status = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isChecked && collision.gameObject.CompareTag("Player") && Type != RoomType.HorizontalCorridor && Type != RoomType.VerticalCorridor)
        {
            _isChecked = true;
            _status.CheckRoom();
            MonsterSpawner.Instance.SummonMonster(SpawnPositionList,_status._checkedRoom);
        }
    
    }
}
