using UnityEngine;

public class Harpoon : Projectile
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override  void Start()
    {
        base.Start();
    }

    protected override  void OnEnable()
    {
        transform.position = _playerTransform.position;
        Vector3 point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camera.transform.position.z));
        _rigidbody.AddForce((point - transform.position).normalized * _flyForce, ForceMode2D.Impulse);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D (collision);
        if(collision != null && collision.CompareTag("Ground")) Disappear();   
    }
    protected override void Disappear()
    {
        _playerTransform.position = gameObject.transform.position;
        gameObject.SetActive(false);
        return;
    }
}
