using UnityEngine;
using static Enums;

public class FlyingMonsterWall : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] MW_AddExternalForceDirection direction;
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("FlyingMonsterWall");
        sr.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        MonsterMovement component;
        collision.TryGetComponent<MonsterMovement>(out component);
        if (component != null) { component.AddingExternalForce(direction); }
    }
}
