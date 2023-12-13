using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class BossMonster : MonoBehaviour
{
    #region Components
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackCircle;
    [SerializeField] private Transform hud;
    [SerializeField] private BossRoom room;
    [SerializeField] private Transform _target;
    #endregion

    #region Variables
    [SerializeField] private bool isOverInit;
    [SerializeField] private bool isReadyNextBehaviour;

    [SerializeField] public float MaxHP;
    [SerializeField] public float Damage;
    [SerializeField] public float MoveSpeed;
    [SerializeField] public float MoveSpeedMultiplier;
    [SerializeField] public float AttackRange;
    [SerializeField] public float AttackInterval;
    [SerializeField] private bool isReadyToAttack;
    [SerializeField] private bool isDirectionRight;

    private bool isRageMode = false;
    private float MoveAcceleration = 1f;
    private int spinNumber;
    public float CurHP;
    private WaitForSeconds MoveInterval_WFS;
    private WaitForSeconds AttackInterval_WFS;
    private float timeFromLastAttack;
    #endregion

    #region Audios
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip AttackAudioClip;
    [SerializeField] private AudioClip RangeAttackAudioClip;
    [SerializeField] private AudioClip SpinAttackAudioClip;
    [SerializeField] private AudioClip RageModeAudioClip;
    [SerializeField] private AudioClip DamageAudioClip;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Sleep");
        isOverInit = false;
        isDirectionRight = true;
        timeFromLastAttack = 0;
    }


    private void FixedUpdate()
    {
        if (!isOverInit) return;
        if (timeFromLastAttack > 8f)
        {
            room.animator.SetTrigger("TrapStart");
            timeFromLastAttack = 0;
        }
        else
        {
            timeFromLastAttack += Time.fixedDeltaTime;
        }
        if (!isReadyNextBehaviour) return;
        NextBehaviour();
    }

    public void Init(Transform player)
    {
        animator.SetTrigger("Awake");
        isReadyNextBehaviour = false;
        isOverInit = true;
        isReadyToAttack = true;
        _target = player;

        MoveInterval_WFS = new WaitForSeconds(0.1f);
        AttackInterval_WFS = new WaitForSeconds(AttackInterval);

        CurHP = MaxHP;
        ApplyFosmonEffects();
    }

    public void NextBehaviour()
    {
        bool targetDirectionIsRight = ((_target.transform.position.x - rb.transform.position.x) > 0) ? true : false;

        if (targetDirectionIsRight != isDirectionRight)
        {
            if (targetDirectionIsRight) { TurnRight(); return; }
            else                        { TurnLeft(); return; }
        }

        float distance = GetTargetSqrDistance();

        if (isReadyToAttack && distance < AttackRange * AttackRange)
        {
            int random = Random.Range(0, 5);
            switch (random)
            {
                case 0: Attack(); timeFromLastAttack = 0; return;
                case 1: RangeAttack(); timeFromLastAttack = 0; return;
                case 2: SpinAttack(); timeFromLastAttack = 0; return;
                case 3: Buff(); timeFromLastAttack = 0; return;
                case 4: Burst(); timeFromLastAttack = 0; return;
                default: break;
            }
        }
        else { Move(); }

    }

    public void TurnLeft()
    {
        rb.velocity = Vector3.zero;
        isReadyNextBehaviour = false;
        isDirectionRight = false;

        animator.SetTrigger("TurnLeft");
    }

    public void TurnRight()
    {
        rb.velocity = Vector3.zero;
        isReadyNextBehaviour = false;
        isDirectionRight = true;

        animator.SetTrigger("TurnRight");
    }

    public void Attack()
    {
        isReadyNextBehaviour = false;
        animator.SetTrigger("Attack");
        isReadyToAttack = false;
        StartCoroutine(nameof(ResetReadyToAttack));
    }

    public void RangeAttack()
    {
        isReadyNextBehaviour = false;
        animator.SetTrigger("RangeAttack");
        isReadyToAttack = false;
        StartCoroutine(nameof(ResetReadyToAttack));
    }

    public void SpinAttack()
    {
        isReadyNextBehaviour = false;
        animator.SetTrigger("SpinAttack");
        spinNumber = 0;
        isReadyToAttack = false;
        StartCoroutine(nameof(ResetReadyToAttack));
    }
    public void CheckSpin()
    {
        spinNumber++;
        if (spinNumber >= 5)
        {
            spinNumber = 0;
            EndSpin();
        }
    }
    public void EndSpin()
    {
        animator.SetTrigger("SpinEnd");
    }
    public void Buff()
    {
        isReadyNextBehaviour = false;
        animator.SetTrigger("Buff");
        isReadyToAttack = false;
        StartCoroutine(nameof(ResetReadyToAttack));
    }

    public void Burst()
    {
        isReadyNextBehaviour = false;
        animator.SetTrigger("Burst");
        isReadyToAttack = false;
        StartCoroutine(nameof(ResetReadyToAttack));
    }

    public void Move()
    {
        isReadyNextBehaviour = false;
        animator.SetTrigger("Move");
    }

    public void EndAnimation()
    {
        isReadyNextBehaviour = true;
    }

    public void ApplyFosmonEffects()
    {
    }


    public float GetTargetSqrDistance()
    {
        float distance;
        distance = (rb.transform.position - _target.position).sqrMagnitude;
        return distance;
    }

    public void StartMove()
    {
        StartCoroutine(nameof(MoveToTarget));
    }

    public void EndMove()
    {
        StopCoroutine(nameof(MoveToTarget));
    }

    public IEnumerator MoveToTarget()
    {
        while (true)
        {
            Vector2 velocity = rb.velocity;
            float moveDirection = (_target.position.x - transform.position.x) > 0 ? 1 : -1;
            float targetSpeed = moveDirection * MoveSpeed * MoveSpeedMultiplier;
            float forceX = (targetSpeed - velocity.x) * MoveAcceleration;
            float forceY = -velocity.y * MoveAcceleration;
            rb.AddForce(new Vector2(forceX, forceY));
            yield return MoveInterval_WFS;
        }
    }

    public void FlipRight()
    {
        Vector3 forFlip = rb.transform.localScale;
        forFlip.x = 1 * Mathf.Abs(forFlip.x);
        rb.transform.localScale = forFlip;
        Vector3 newScale = hud.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * 1;
        hud.transform.localScale = newScale;
    }

    public void FlipLeft()
    {
        Vector3 forFlip = rb.transform.localScale;
        forFlip.x = -1 * Mathf.Abs(forFlip.x);
        rb.transform.localScale = forFlip;
        Vector3 newScale = hud.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * -1;
        hud.transform.localScale = newScale;
    }

    public IEnumerator ResetReadyToAttack()
    {
        yield return AttackInterval_WFS;
        isReadyToAttack = true;
    }

    public void BeAttacked(float attack)
    {
        CurHP -= attack * 0.1f + 2;
        if (CurHP <= 0)
        {
            isReadyNextBehaviour = false;
            rb.velocity = Vector3.zero;
            MoveSpeed = 0f;
            animator.SetTrigger("Death");
        }

        if (!isRageMode && (CurHP * 2 - MaxHP) < 0)
        {
            AudioManager.Instance.StopBGM();
            StartRageMode();
        }
    }

    public void StartRageMode()
    {
        isRageMode = true;
        AudioManager.Instance.PlayBGM(BGMClips.AngryBossBGM);
        MoveSpeed *= 1.5f;
        AttackInterval_WFS = new WaitForSeconds(AttackInterval * 0.5f);
        spriteRenderer.color = Color.red;
        AttackAudio(RageModeAudioClip);
    }
    public void AttackAudio(AudioClip clip)
    {
        if (_audioSource.isPlaying) { _audioSource.Stop(); }
        _audioSource.clip = clip;
        _audioSource.Play();
    }
    public void PlayerWin()
    {
        LoadingSceneController.LoadScene(SceneName.MovieScene_Win);
    }

    #region Audio Method
    public void PlaySpinAttackAudio()
    {
        _audioSource.loop = true;
        AttackAudio(SpinAttackAudioClip);
    }

    public void AudioSpeedUP()
    {
        _audioSource.pitch += .2f;
    }
    public void StopSpinAttackAudio()
    {
        _audioSource.loop = false;
        _audioSource.pitch = 1;
    }

    public void PlayAttackAudio()
    {
        AttackAudio(AttackAudioClip);
    }

    public void PlayRangeAttackAudio()
    {
        AttackAudio(RangeAttackAudioClip);
    }

    public void PlayBuffAudio()
    {
        AttackAudio(RageModeAudioClip);
    }
    #endregion
}