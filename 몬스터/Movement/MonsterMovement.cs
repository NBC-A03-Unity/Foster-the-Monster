using System;
using System.Collections;
using UnityEngine;
using static Enums;

public abstract class MonsterMovement : MonoBehaviour
{
    #region Components
    [Header("Components")]
    [SerializeField] protected Monster monster;
    protected Rigidbody2D _rigid;
    protected Collider2D _collider;
    [HideInInspector] public Animator _animator;
    #endregion

    #region Variables
    [Header("Movement Variables")]
    protected float moveSpeed;
    protected float moveSpeedMultiplier;
    protected float moveAcceleration;
    protected float moveForce;

    protected float attackRange;
    protected float attackInterval;
    protected float wanderingDirectionInterval = 2f;
    protected float attackingTime = 1f;
    #endregion

    #region Inspect
    protected LayerMask groundLayer;
    public Transform curTarget;
    [SerializeField] protected MonsterBehaviourState curState;

    protected WaitForSeconds directionChangeIntervalWFS;
    protected WaitForSeconds attackingWFS;
    protected WaitForSeconds attackIntervalWFS;

    public bool isDirectionRight;
    public bool isReadyToAttack;
    public bool isReadyToChangeDirection;
    public bool isInAttackRange;
    public bool isAttacking;

    protected bool isStateChanged;
    #endregion

    protected virtual void Awake()
    {
        monster = GetComponent<Monster>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        monster.OnCurStateChanged += ReactCurStateChanged;
    }
    protected virtual void OnDestroy()
    {
        monster.OnCurStateChanged -= ReactCurStateChanged;
    }
    protected virtual void Update()
    {
        SpriteFlip();
    }

    protected virtual void FixedUpdate()
    {
        if (isStateChanged)
        {
            SetAnimatorBools();
            isStateChanged = false;
        }
        CheckTargetIsInAttackRange();
        AttackIfPossible();
        Behaviour();
    }
    protected virtual void SetAnimatorBools()
    {
        switch (curState)
        {
            case MonsterBehaviourState.Wandering:
                _animator.SetBool("IsChasing", false);
                _animator.SetBool("IsFleeing", false);
                moveSpeedMultiplier = 0.5f;
                break;
            case MonsterBehaviourState.Chasing:
                _animator.SetBool("IsChasing", true);
                _animator.SetBool("IsFleeing", false);
                moveSpeedMultiplier = 1f;
                AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.SM_Alerted);
                break;
            case MonsterBehaviourState.Fleeing:
                _animator.SetBool("IsChasing", false);
                _animator.SetBool("IsFleeing", true);
                moveSpeedMultiplier = 1.2f;
                break;
            case MonsterBehaviourState.Rage_Chasing:
                _animator.SetBool("IsChasing", true);
                _animator.SetBool("IsFleeing", false);
                moveSpeedMultiplier = 1.8f;
                break;
            case MonsterBehaviourState.Happy_Chasing:
                _animator.SetBool("IsChasing", true);
                _animator.SetBool("IsFleeing", false);
                moveSpeedMultiplier = 1.8f;
                break;
            default:
                break;
        }

    }

    protected virtual void CheckTargetIsInAttackRange()
    {
        if (curTarget != null)
        {
            if ((curTarget.position.x - transform.position.x) * (curTarget.position.x - transform.position.x)
                < (attackRange * attackRange * 0.25))
            {
                isInAttackRange = true;
                _animator.SetBool("IsInRange", true);
            }
            else
            {
                isInAttackRange = false;
                _animator.SetBool("IsInRange", false);
            }
        }
        else
        {
            isInAttackRange = false;
            _animator.SetBool("IsInRange", false);
        }
    }
    protected virtual void AttackIfPossible()
    {
        if (monster.monsterMode == MonsterMode.Happy) { return; }
        if (curTarget != null && curTarget.CompareTag("Player") && isReadyToAttack && isInAttackRange)
        {
            _animator.SetTrigger("Attack");
            AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.SM_Attack);
            isAttacking = true;
            isReadyToAttack = false;
            StartCoroutine(ActionAfterTime(() => { isAttacking = false; }, attackingWFS));
            StartCoroutine(ActionAfterTime(() => { isReadyToAttack = true; }, attackIntervalWFS));
        }
    }
    public virtual void Init(MonsterSO monsterSO)
    {
        gameObject.SetActive(true);

        moveSpeed = monsterSO.moveSpeed;
        moveForce = monsterSO.moveForce;
        moveAcceleration = monsterSO.moveAcceleration;
        moveSpeedMultiplier = 1f;

        attackRange = monsterSO.attackRange;
        attackInterval = monsterSO.attackInterval;

        wanderingDirectionInterval = 5f / monsterSO.moveSpeed;

        isReadyToAttack = true;
        isReadyToChangeDirection = true;

        directionChangeIntervalWFS = new WaitForSeconds(wanderingDirectionInterval);
        attackIntervalWFS = new WaitForSeconds(attackInterval);
        attackingWFS = new WaitForSeconds(attackingTime);
    }

    protected virtual void ReactCurStateChanged(MonsterBehaviourState newState, Transform newTarget)
    {
        curState = newState;
        curTarget = newTarget;
        isStateChanged = true;
    }

    protected virtual void SpriteFlip()
    {
        Vector2 forSpriteFlip = monster.transform.localScale;
        forSpriteFlip.x = _rigid.velocity.x < -0.0001f ? 1 : forSpriteFlip.x;
        forSpriteFlip.x = _rigid.velocity.x > 0.0001f ? -1 : forSpriteFlip.x;
        monster.transform.localScale = forSpriteFlip;
    }

    protected virtual void Behaviour()
    {
        if (isInAttackRange || isAttacking)
        {
            Stop();
            return;
        }
        else if (curState == MonsterBehaviourState.Wandering)
        {
            if (isReadyToChangeDirection)
            {
                isDirectionRight = !isDirectionRight;
                isReadyToChangeDirection = false;
                StartCoroutine(ActionAfterTime(() => { isReadyToChangeDirection = true; }, directionChangeIntervalWFS));
            }
            Vector2 newTarget = isDirectionRight ? Vector2.right : Vector2.left;
            newTarget += (Vector2)monster.transform.position;
            Move(newTarget);
            return;
        }
        else if (curTarget != null)
        {
            if (curState == MonsterBehaviourState.Chasing || curState == MonsterBehaviourState.Rage_Chasing || curState == MonsterBehaviourState.Happy_Chasing)
            {
                Move(curTarget.position, monster.monsterSO.isFlyable);
                return;
            }
            if (curState == MonsterBehaviourState.Fleeing)
            {
                Flee(curTarget.position);
                return;
            }
        }
    }

    public virtual void Move(Vector2 target, bool isflyable = false)
    {
        if (!isflyable)
        {
            Vector2 velocity = _rigid.velocity;
            float moveDirection = (target.x - transform.position.x) > 0 ? 1 : -1;
            float targetSpeed = moveDirection * moveSpeed * moveSpeedMultiplier;
            float forceX = (targetSpeed - velocity.x) * moveAcceleration;
            forceX = Mathf.Clamp(forceX, -moveForce, moveForce);
            float forceY = -velocity.y * moveAcceleration;
            _rigid.AddForce(new Vector2(forceX, forceY));
        }
        else
        {
            Vector2 velocity = _rigid.velocity;
            Vector2 moveDirection = target - (Vector2)(transform.position);
            moveDirection.Normalize();
            Vector2 targetSpeed = moveDirection * moveSpeed * moveSpeedMultiplier;
            Vector2 force = (targetSpeed - velocity) * moveAcceleration;
            force.x = Mathf.Clamp(force.x, -moveForce, moveForce);
            force.y = Mathf.Clamp(force.y, -moveForce, moveForce);
            _rigid.AddForce(force);
        }
    }
    public virtual void Flee(Vector2 target)
    {
        Vector2 velocity = _rigid.velocity;
        float moveDirection = (target.x - transform.position.x) > 0 ? -1 : 1;
        float targetSpeed = moveDirection * moveSpeed * moveSpeedMultiplier;
        float forceX = (targetSpeed - velocity.x) * moveAcceleration;
        forceX = Mathf.Clamp(forceX, -moveForce, moveForce);
        float forceY = -velocity.y * moveAcceleration;
        _rigid.AddForce(new Vector2(forceX, forceY));
    }
    public virtual void Stop()
    {
        Vector2 velocity = _rigid.velocity;
        float forceX = -velocity.x * moveAcceleration;
        forceX = Mathf.Clamp(forceX, -moveForce, moveForce);
        float forceY = -velocity.y * moveAcceleration;
        _rigid.AddForce(new Vector2(forceX, forceY));
    }

    public IEnumerator ActionAfterTime(Action callback, WaitForSeconds time)
    {
        yield return time;
        callback();
        yield return null;
    }

    public virtual void AddingExternalForce(MW_AddExternalForceDirection direction) 
    {
        float currentSpeed = _rigid.velocity.magnitude;
        float force = (moveSpeed - currentSpeed) / Time.fixedDeltaTime;

        Vector2 directionVector;
        switch (direction)
        {
            case MW_AddExternalForceDirection.Up: directionVector = Vector2.up; break;
            case MW_AddExternalForceDirection.Down: directionVector = Vector2.down; break;
            case MW_AddExternalForceDirection.Left: directionVector = Vector2.left; break;
            case MW_AddExternalForceDirection.Right: directionVector = Vector2.right; break;
            case MW_AddExternalForceDirection.UpAndDown:
                directionVector = (_rigid.velocity.y >0)? Vector2.up: Vector2.down; break;
            case MW_AddExternalForceDirection.LeftAndRight:
                directionVector = (_rigid.velocity.x >0)? Vector2.right: Vector2.left; break;
            default:
                directionVector = Vector2.zero; 
                break;
        }
        _rigid.AddForce(directionVector * force, ForceMode2D.Force);
    }
}