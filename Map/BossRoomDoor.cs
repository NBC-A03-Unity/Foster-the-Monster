using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] AudioClip OpenSound;
    [SerializeField] AudioClip CloseSound;
    [SerializeField] AudioSource _audioSource;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _animator.SetBool("IsOpen", true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _animator.SetBool("IsOpen", false);
        }
    }

    private void PlayOpenSound()
    {
        _audioSource.clip = OpenSound;
        _audioSource.Play();
    }

    private void PlayCloseSound()
    {
        _audioSource.clip = CloseSound;
        _audioSource.Play();
    }
}