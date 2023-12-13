using System.Collections;
using UnityEngine;
using static Enums;

public class Melee : MonoBehaviour
{
    private Transform _playerTransform;
    private Player _player;
    private Rigidbody2D _rigidbody;



    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _player = DataManager.Instance.Player;
        if (!_player.IsSwinging)
            StartCoroutine(SwingWeapon());
    }

    private void OnEnable()
    {
        _player = DataManager.Instance.Player;
        if (!_player.IsSwinging)
        {
            _player.IsSwinging = true;
            StartCoroutine(SwingWeapon());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Monster"))
        {
            AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.EnemyHit);
            if (_player.IsTackle)
            {
                collision.GetComponent<Monster>().BeAttacked(_player.Attack * _rigidbody.velocity.magnitude * 0.1f, _playerTransform);
            }
            else
            {
                collision.GetComponent<Monster>().BeAttacked(_player.Attack, _playerTransform);
                StopCoroutine(SwingWeapon());
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.CompareTag("BossHitBox"))
        {
            AudioManager.Instance.PlaySFX(SFXCategory.Character, SFXClips.EnemyHit);
            if (_player.IsTackle)
            {
                collision.GetComponentInParent<BossMonster>().BeAttacked(_player.Attack*2);
            }
            else
            {
                collision.GetComponentInParent<BossMonster>().BeAttacked(_player.Attack);
                StopCoroutine(SwingWeapon());
                gameObject.SetActive(false);

            }
        }
        
    }

    private IEnumerator SwingWeapon()
    {
        while (_player.IsSwinging)
        {
            yield return null;
        }
        StopCoroutine(SwingWeapon());
        gameObject.SetActive(false);
    }
}
