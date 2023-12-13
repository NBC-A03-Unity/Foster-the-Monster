using UnityEngine;
using UnityEngine.Pool;
using static Enums;

public class Projectile : MonoBehaviour
{
    protected Transform _playerTransform;
    protected Camera _camera;
    protected Rigidbody2D _rigidbody;
    protected Player _player;
    protected SpriteRenderer _projectileSprite;

    protected IObjectPool<Projectile> _projectilePool;

    public IObjectPool<Projectile> ProjectilePool { set => _projectilePool = value;  }

    [SerializeField] protected float _flyForce;


    protected virtual void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform;
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _projectileSprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        _player = DataManager.Instance.Player;
    }

    protected virtual void OnEnable()
    {
        
        transform.position = _playerTransform.position;
        Vector3 point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, - _camera.transform.position.z));
        Vector3 flyVector = (Mathf.Abs(point.y - transform.position.y) / Mathf.Abs(point.x- transform.position.x) < .5f)? 
            point - transform.position : new Vector3(point.x - transform.position.x, (point.y>transform.position.y)? 
            .5f *Mathf.Abs(point.x - transform.position.x) : -.5f*Mathf.Abs(point.x - transform.position.x), point.z - transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(flyVector.y, flyVector.x) * Mathf.Rad2Deg);
        FlipSprite();
        _rigidbody.AddForce(flyVector.normalized *_flyForce, ForceMode2D.Impulse);
        Invoke(nameof(Disappear), 1f);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Monster"))
        {
            collision.GetComponent<Monster>().BeAttacked(_player.Attack,_playerTransform);
            AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.EnemyHit);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("BossHitBox")) 
        {
            collision.GetComponentInParent<BossMonster>().BeAttacked(_player.Attack);
            AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.EnemyHit);
            gameObject.SetActive(false);
        }
    }

    protected void FlipSprite()
    {
        _player = DataManager.Instance.Player;
        _projectileSprite.flipY = (Input.mousePosition.x > 960) ? false : true;
    }

    protected virtual void Disappear()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
        _rigidbody.velocity = Vector3.zero;

        _projectilePool?.Release(this);
    }
}
