using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static Enums;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    #region Components
    public Player Player { get; private set; }
    public PlayerInputActions PlayerInput;
    public PlayerInputActions.PlayerControlActions PlayerControlActions {  get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public CapsuleCollider2D TerrainCollider { get; private set; }
    public BoxCollider2D TriggerCollider { get; private set; }
    public Weapon Weapon { get; private set; }
    [SerializeField] private Transform _playerSprite;
    [SerializeField] private Transform _weaponSprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _jumpUI;
    [SerializeField] private GameObject _extraCount;
    [SerializeField] private Sprite[] _jumpCountSprites;
    [SerializeField] private SpriteRenderer _jumpCountSprite;

    [SerializeField] private GameObject _noWeaponText;
    [SerializeField] private GameObject _noItemText;
    [SerializeField] private GameObject _noJumpCountText;

    public AssetReferenceGameObject ProjectilePrefab;

    private Camera _camera;
    #endregion

    #region Animation Parameters
    private const string _jumpTrigger = "jump";
    private const string _hitTrigger = "hit";
    private const string _fallTrigger = "fall";
    private const string _attackTrigger = "attack";
    private const string _boostTrigger = "boost";
    private const string _isMove = "isMove";
    private const string _isDuck = "isDuck";
    private const string _isShoot = "isShoot";
    private const string _isGround = "isGround";
    private const string _isTackle = "isTackle";
    private const string _evadeTrigger = "evade";
    #endregion

    #region Variables
    private Vector2 _moveInput;

    [SerializeField] private float _moveAccel;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _knockBack;
    [SerializeField] private float _throwForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _resetLayer;
    [SerializeField] private LayerMask _monsterLayer;
    [SerializeField] private Vector3 _throwOffset;
    private Vector3 _leftVector = new Vector3(-1, 1, 1);
    
    private float _initialGravity = 0f;
    private bool _isDownJumping;
    private bool _isEvading;
    private bool _inventoryOpen;
    private bool _isAttack;
    private bool _isHelp;

    private int _maxJumpCount = 1;
    private float _maxSpeed = 0;
    private int _jumpCount = 0;

    private float _tempSpeed = 0f;

    private WeaponType _weaponType;

    private float _attackCool;
    private float _timeSinceLastAttack;
    #endregion

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        TerrainCollider = GetComponent<CapsuleCollider2D>();
        TriggerCollider = GetComponent<BoxCollider2D>();
        PlayerInput = new PlayerInputActions();
        PlayerControlActions = PlayerInput.PlayerControl;
        GameManager.Instance.PlayerController = this;

        _camera = Camera.main;  
    }
    private void Start()
    {
        if (DataManager.Instance.Player != null)
        {
            Player = DataManager.Instance.Player;
            _maxJumpCount = Player.MaxJump;
            _maxSpeed = Player.MoveSpeed;
        }
        _jumpCount = _maxJumpCount;
        _initialGravity = Rigidbody.gravityScale;
        
        PlayerControlActions.Movement.started += (context) =>
        {
            StartCoroutine(GetMovementInput(context));
        };
        PlayerControlActions.Movement.canceled += (context)=>
        {
            StopCoroutine(GetMovementInput(context));
            _moveInput = Vector2.zero;
            _animator.SetBool(_isMove, false);
            _animator.SetBool(_isDuck, false);
        };
        PlayerControlActions.Shoot.started += _ =>
        {
            if (Weapon != null && Time.timeScale != 0f)
            {
                if(_weaponType == WeaponType.CloseNet)
                {
                    _animator.SetTrigger(_attackTrigger);
                    Weapon.PerformCatch();
                    SFXClips clipToPlay = SFXClips.Swing;

                    AudioManager.Instance.PlaySFX(SFXCategory.Character, clipToPlay);

                }
                else if(_weaponType == WeaponType.BasicNet || _weaponType == WeaponType.Harpoon)
                {
                    _isAttack = true;
                    _animator.SetBool(_isShoot, true);
                    StartCoroutine(ShootProjectile());
                } 
            }
            else
            {
                _noWeaponText.SetActive(true);
                this.Invoke(() => _noWeaponText.SetActive(false), 1.5f);
            }
        };
        PlayerControlActions.Shoot.canceled += _ =>
        {
            _isAttack = false;
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) Player.IsSwinging = false;
            _animator.SetBool(_isShoot, false);
            StopCoroutine(ShootProjectile());
        };
        PlayerControlActions.Skill.started += _ =>
        {

            if (Weapon != null && _weaponType == WeaponType.Harpoon && Time.timeScale != 0f)
            {
                Weapon.PerformSpecial();
            }
            else if(Weapon != null && _weaponType == WeaponType.CloseNet && Time.timeScale != 0f)
            {
                SFXClips clipToPlay = SFXClips.Swing;
                Player.IsTackle = true;
                if (Player.IsTackle && _jumpCount > 0)
                {
                    _animator.SetBool(_isTackle, true);
                    _animator.SetTrigger(_boostTrigger);

                    clipToPlay = SFXClips.Tackle;

                    Weapon.PerformSpecial();

                    _jumpCount--;
                    UpdateJumpCount();
                }
                else
                {
                    _noJumpCountText.gameObject.SetActive(true);
                    this.Invoke(() => _noJumpCountText.gameObject.SetActive(false), 1.5f);
                }
                AudioManager.Instance.PlaySFX(SFXCategory.Character, clipToPlay);
                _timeSinceLastAttack = 0;
            }
            else if (Player.ThrowableItem != null)
            {
                if (Player.ThrowItem(Player.ThrowableItem))
                {
                    Item item = Instantiate(ResourceManager.Instance.LoadPrefab<Item>($"Items/{Player.ThrowableItem.ItemID}", Player.ThrowableItem.ItemName));
                    item.transform.position = (Player.IsRight) ? transform.position + _throwOffset : transform.position - _throwOffset;
                    item.isThrown = true;
                    Destroy(item.gameObject, 3f);
                    Vector3 point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camera.transform.position.z));
                    item.GetComponent<Rigidbody2D>().AddForce((point - transform.position).normalized * _throwForce, ForceMode2D.Impulse);
                }
                else
                {
                    _noItemText.SetActive(true);
                    this.Invoke(() => _noItemText.SetActive(false), 1.5f);
                }
                
            }
            else
            {
                _noItemText.SetActive(true);
                this.Invoke(() => _noItemText.SetActive(false), 1.5f);
            }
            
        };
        PlayerControlActions.Skill.canceled += _ =>
        {
            _isAttack = false;
            _animator.SetBool(_isShoot, false);
            _animator.SetBool(_isTackle, false);
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                Player.IsSwinging = false;
                Player.IsTackle = false;
                _maxSpeed = _tempSpeed > 0 ? _tempSpeed : _maxSpeed;
                _tempSpeed = 0;
            }
            StopCoroutine(ShootProjectile());
        };
        PlayerControlActions.Jump.started += _ =>
        {
            Player.IsTackle = false;
            _animator.SetBool(_isGround, false);
            Jump();
        };
        PlayerControlActions.Inventory.started += _ =>
        {
            if(Player.CurrentTenacity > 0)
            {
                if (!_inventoryOpen)
                {
                    Time.timeScale = 0f;
                    UIManager.Instance.OpenUI<InventoryUI>();
                    _inventoryOpen = true;
                }
                else
                {
                    ApplyPlayerStats();
                } 
            }
        };
        PlayerControlActions.Evade.started += _ =>
        {
            if(!_isEvading)
            {
                AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Dash);
                _isEvading = true;
                Rigidbody.velocity = Vector2.zero;
                _animator.SetTrigger(_evadeTrigger);
                Vector2 dir = (Player.IsRight) ? Vector2.right : Vector2.left;
                Rigidbody.AddForce(dir * _knockBack, ForceMode2D.Impulse);
                TriggerCollider.excludeLayers = _monsterLayer;
                StartCoroutine(CheckAnimationEnd());
            }
        };
        PlayerControlActions.SwitchWeapon.started += _ =>
        {
            InventoryUI inventoryUI = UIManager.Instance.OpenUI<InventoryUI>();
            foreach (Item weapon in Player.Inventory[ItemType.Weapon].Values)
            {
                if (((WeaponSO)weapon.ItemSO).WeaponType != _weaponType)
                {
                    inventoryUI.SelectedItem = weapon.ItemSO;
                    inventoryUI.EquipItem();
                    break;
                }
            }
            UIManager.Instance.CloseUI<InventoryUI>();
        };
        PlayerControlActions.OpenHelp.started += _ =>
        {
            if(!_isHelp)
            {
                Time.timeScale = 0f;
                UIManager.Instance.OpenUI<UIHelp>();
                _isHelp = true;
            }
            else
            {
                UIManager.Instance.CloseUI<UIHelp>();
                Time.timeScale = 1f;
                _isHelp = false;
            }
            
        };


        StartCoroutine(CheckJump(new WaitForSeconds(0.05f)));
    }

    private void OnEnable()
    {
        PlayerInput.Enable();
    }

    private void OnDisable()
    {
        PlayerInput.Disable();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void Update()
    {
        if (!Player.IsSwinging) _animator.SetBool(_isTackle, false);

        if (_timeSinceLastAttack < _attackCool) _timeSinceLastAttack += Time.deltaTime;
    }

    private void ApplyMovement()
    {
        if (Rigidbody != null && _moveInput != Vector2.zero)
        {
            if (Math.Abs(Rigidbody.velocity.x) < _maxSpeed)
                Rigidbody.AddForce(_moveInput.normalized.x * Vector2.right * _moveAccel, ForceMode2D.Force);
        }

        if(Mathf.Abs(Rigidbody.velocity.y) > 30f)
        {
            Rigidbody.gravityScale = 1.0f;
        }

        if(Mathf.Abs(Rigidbody.velocity.magnitude) > 20f)
        {
            int layerMask = 1 << 16;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Math.Abs(Rigidbody.velocity.x) > Math.Abs(Rigidbody.velocity.y)? (Rigidbody.velocity.x > 0?Vector2.right : Vector2.left) : Vector2.down, 1f, layerMask);
            Rigidbody.velocity = hit.collider != null ? Vector2.zero : Rigidbody.velocity;
        }
    }

    private void Jump()
    {
        if (Rigidbody != null && _jumpCount > 0)
        {

            if(_jumpCount == _maxJumpCount)
                AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Jump);
            else AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Boost);
            Rigidbody.gravityScale = _initialGravity;
            Rigidbody.velocity = Vector2.zero;
            if (_moveInput.y >= 0)
            {
                _animator.SetTrigger(_jumpTrigger);
                StartCoroutine(CheckFall(new WaitForSeconds(0.1f)));
                Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                _animator.SetTrigger(_fallTrigger);
                _isDownJumping = true;
                this.Invoke(() => _isDownJumping = false, 0.3f);
            }
            _jumpCount--;
            UpdateJumpCount();
        }
    }


    private void UpdateJumpCount()
    {
        _jumpUI.SetActive(true);


        switch (_jumpCount)
        {
            case 0:
                _jumpCountSprite.sprite = null;
                _extraCount.SetActive(false);
                break;
            case 1:
                _jumpCountSprite.sprite = _jumpCountSprites[2];
                _extraCount.SetActive(false);
                break;
            case 2:
                _jumpCountSprite.sprite = _jumpCountSprites[1];
                _extraCount.SetActive(false);
                break;
            case 3:
                _jumpCountSprite.sprite = _jumpCountSprites[0];
                _extraCount.SetActive(false);
                break;
            default:
                _jumpCountSprite.sprite = _jumpCountSprites[0];
                _extraCount.SetActive(true);
                break;

        }
    }

    private IEnumerator CheckFall(WaitForSeconds interval)
    {
        float fallingTime = 0f;
        while(_jumpCount < _maxJumpCount)
        {
            fallingTime += 0.1f;
            if(fallingTime > 5f)
            {
                Rigidbody.velocity = Vector2.zero;
                Player.DecreaseTenacity(30);
                gameObject.transform.position = gameObject.GetComponent<PlayerStatus>().RespawnPos;
            }

            if(Rigidbody.velocity.y < 0)
            {
                Rigidbody.gravityScale = 1.0f;
                _animator.SetTrigger(_fallTrigger);
                break;
            }
            yield return interval;
        }
    }

    public void ApplyKnockBack(bool isRight)
    {
        Rigidbody.velocity = Vector2.zero;
        _animator.SetTrigger(_hitTrigger);
        var dir = (isRight) ? Vector2.right : Vector2.left;
        Rigidbody.AddForce(dir * _knockBack, ForceMode2D.Impulse);        
    }

    public void ChangeWeapon()
    {
        _weaponType = (Player != null) ? Player.WeaponType : WeaponType.None;
        switch (_weaponType)
        {
            case WeaponType.BasicNet:
                Weapon = gameObject.AddComponent<BasicNet>();
                break;
            case WeaponType.CloseNet:
                Weapon = gameObject.AddComponent<CloseNet>();
                break;
            case WeaponType.Harpoon :
                Weapon = gameObject.AddComponent<HarpoonThrow>();
                break;
            default:
                Weapon = null;
                break;

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool(_isGround, true);
            _jumpCount = _maxJumpCount;
            if(_jumpUI.gameObject.activeSelf)
            {
                UpdateJumpCount();
                this.Invoke(() =>
                {
                    _extraCount.SetActive(false);
                    _jumpUI.SetActive(false);
                }
                , 2f);
            }
            Rigidbody.gravityScale = _initialGravity;
        }
    }



    private IEnumerator CheckJump(WaitForSeconds interval)
    {
        while (true)
        {
            if(Rigidbody != null && Rigidbody.velocity.y > 0 || _isDownJumping)
            {
                TerrainCollider.excludeLayers = _groundLayer;
            }
            else
            {
                TerrainCollider.excludeLayers = _resetLayer;
            }

            yield return interval;
        }
    }

    private IEnumerator GetMovementInput(InputAction.CallbackContext context)
    {
        float audioPlayTime = 1f;
        _moveInput = context.ReadValue<Vector2>();
        while (_moveInput != Vector2.zero)
        {
            if(audioPlayTime > 0.5f & _jumpCount == _maxJumpCount)
            {
                AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Run);
                audioPlayTime = 0f;
            }
            
            audioPlayTime += Time.deltaTime;
            _animator.SetBool(_isMove, false);
            _moveInput = context.ReadValue<Vector2>();
            if (Time.timeScale != 0f)
            {
                _animator.SetBool(_isMove, true);
                if (_moveInput.x != 0 & !_isAttack)
                {
                    if (_moveInput.x < 0)
                    {
                        _playerSprite.localScale = _leftVector;
                        Player.IsRight = false;
                    }
                    else if (_moveInput.x > 0)
                    {
                        _playerSprite.localScale = Vector3.one;
                        Player.IsRight = true;
                    }
                }
                else if (_moveInput.y < 0) _animator.SetBool(_isDuck, true);
            }
            yield return null;
        }
    }

    private IEnumerator CheckAnimationEnd()
    {
        while(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        this.Invoke(() => 
        {
            _isEvading = false;
            TriggerCollider.excludeLayers = _resetLayer;
        }, .5f);
    }

    private IEnumerator ShootProjectile()
    {
        while (_isAttack)
        {
            if (Weapon != null && Time.timeScale != 0f)
            {
                Vector3 point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camera.transform.position.z));
                _playerSprite.localScale = (point.x > transform.position.x) ? Vector3.one : _leftVector;
                if (_timeSinceLastAttack >= _attackCool)
                {
                    AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.Shoot);
                    Weapon.PerformCatch();
                    _timeSinceLastAttack = 0;
                }
            }
            yield return null;
        }
    }

    public void ApplyPlayerStats()
    {
        Time.timeScale = 1f;
        UIManager.Instance.CloseUI<InventoryUI>();
        _inventoryOpen = false;
        Player = DataManager.Instance.Player;
        _maxJumpCount = Player.MaxJump;
        _attackCool = 1f / Player.AttackSpeed;
        _timeSinceLastAttack = _attackCool;
    }
}
