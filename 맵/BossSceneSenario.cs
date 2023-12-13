using UnityEngine;

public class BossSceneSenario : MonoBehaviour
{
    [SerializeField] public BossMonster _bossMonster;
    [SerializeField] public Collider2D _collider;
    [SerializeField] public CameraController _cameraController;
    [SerializeField] public BoxCollider2D bossRoomBounds;
    [SerializeField] public BossRoom bossRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            _bossMonster.Init(collision.gameObject.transform);
            _collider.enabled = false;
            _cameraController.SetBoundary(bossRoomBounds);
            bossRoom.SetTarget(collision.gameObject.transform);
        }
    }
}
