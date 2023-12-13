using UnityEngine;

public class LandMonsterWall : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("LandMonsterWall");
        sr.enabled = false;
    }
}
