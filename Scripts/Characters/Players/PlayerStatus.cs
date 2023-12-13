using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class PlayerStatus : MonoBehaviour
{
    public Player Player { get; private set; }
    public event Action DesynchronizeEvent;

    #region Player Stats
    [Header("Stats")]
    [SerializeField] private int _tenacity;
    [SerializeField] private int _hp;
    [SerializeField] private int _stress;
    [SerializeField] private int _maxJump;
    [SerializeField] private int _maxSpeed;
    [SerializeField] private int _gold;
    [SerializeField] private WeaponType _weaponType;

    private float _tempAttack;
    private float _tempAttackSpeed;
    #endregion

    #region Keys
    private const string _doorKey = "Door";
    private const string _monsterKey = "Monster";
    private const string _roomKey = "Room";
    private const string _itemKey = "Item";
    #endregion

    #region Components
    [SerializeField] private RectTransform _tenacityUI;
    private DataManager _dataManager;
    private PlayerController _controller;
    private Rigidbody2D _rigidbody;
    //[SerializeField] private UIResult _resultUI;
    #endregion

    #region Variables
    private float _hitCoolCount;
    public float _time;
    public int _checkedRoom = 0;

    [Header("Float")]
    [SerializeField] private float _hitCoolInterval;
    [SerializeField] private float _tenacityDecreaseInterval;

    public Vector3 RespawnPos { get; private set; } = Vector3.zero;
    private float _deathLine = float.MaxValue;
    #endregion
    private void Awake()
    {
        UIManager.Instance.OpenUI<UIResult>().ConnectPlayerStatus(this);
        Time.timeScale = 1f;
        _dataManager = DataManager.Instance;
        _controller = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody2D>();   
        if (_dataManager.Player == null)
        {
            Player = new Player(_tenacity, _hp, _stress, _maxJump, _weaponType, _maxSpeed, _gold);
            _dataManager.SavePlayerData(Player);
        }
        else
        {
            _dataManager.UpdatePlayerStat();
            Player = _dataManager.Player;
            Player.SetTenacity(Player.MaxTenacity).SetInventory();
            _tempAttack = Player.Attack;
            _tempAttackSpeed = Player.AttackSpeed;
        }
    }
    private void Start()
    {
        _dataManager.CaughtMonsters.Clear();
        
        DesynchronizeEvent += OnDesychronizeEvent;
        _hitCoolCount = _hitCoolInterval;
        _controller.ChangeWeapon();

        GameObject[] emptyDoors = GameObject.FindGameObjectsWithTag(_doorKey);
        foreach(GameObject door in emptyDoors)
        {
            if (door.transform.position.y < _deathLine) _deathLine = door.transform.position.y;
        }

        StartCoroutine(DecreaseTenacity(new WaitForSecondsRealtime(_tenacityDecreaseInterval)));
    }

    

    private void Update()
    {
        _time += Time.deltaTime;

        if(_hitCoolCount < _hitCoolInterval)
        {
            _hitCoolCount += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (_hitCoolCount >= _hitCoolInterval)
            {
                AttackCircle attack;
                collision.TryGetComponent<AttackCircle>(out attack);
                if (attack != null)
                {
                    _hitCoolCount = 0;
                    AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Hit);
                    _controller.ApplyKnockBack(attack.GetDirection());
                    Player.DecreaseTenacity(attack.GetDamage());
                    _tenacityUI.localScale = new Vector3((float)Player.CurrentTenacity / Player.MaxTenacity, 1, 1);
                    attack.TurnOffAttackCircle();
                }
            }
            if (_hitCoolCount >= _hitCoolInterval)
            {
                if (collision.gameObject.CompareTag("BossAttackBox"))
                {
                    _hitCoolCount = 0;
                    AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Hit);
                    _controller.ApplyKnockBack((collision.transform.position.x - transform.position.x) > 0 ? false:true);
                    Player.DecreaseTenacity(10);
                    _tenacityUI.localScale = new Vector3((float)Player.CurrentTenacity / Player.MaxTenacity, 1, 1);
                }
            }

            if (collision.gameObject.CompareTag(_roomKey))
            {
                RespawnPos = collision.transform.position;
            }
        }
        
    }

    private IEnumerator DecreaseTenacity(WaitForSecondsRealtime interval)
    {
        while (Player.CurrentTenacity > 0)
        {
            if (Time.timeScale != 0f)
            {
                Player.DecreaseTenacity(1);
                
                if(gameObject.transform.position.y < _deathLine -10)
                {
                    _rigidbody.velocity = Vector2.zero;
                    Player.DecreaseTenacity(30);
                    gameObject.transform.position = RespawnPos;
                }
                _tenacityUI.localScale = new Vector3((float)Player.CurrentTenacity / Player.MaxTenacity, 1, 1);
                yield return interval;
            }
            else
            {
                yield return null;
            }
        }
        DesynchronizeEvent?.Invoke();
    }


    private void OnDesychronizeEvent()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Death);
        UIManager.Instance.CloseUI<InventoryUI>();
        if(SceneManager.GetActiveScene().buildIndex==3)
        {
            LoadingSceneController.LoadScene(SceneName.MovieScene_Lose);
        }
        else
        {
            Time.timeScale = 0f;
            UIManager.Instance.OpenUI<UIResult>();
        }
    }
    

    public void CheckRoom()
    {
        _checkedRoom++;
    }

    private void OnDestroy()
    {
        Player.SetTenacity(Player.MaxTenacity).SetInventory();
        Player.Attack = _tempAttack;
        Player.AttackSpeed = _tempAttackSpeed;

        _dataManager.SavePlayerData(Player);
    }
}
