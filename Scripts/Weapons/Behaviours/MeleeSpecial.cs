using UnityEngine;

public class MeleeSpecial : ISpecialBehaviour
{
    private GameObject _weapon;
    private Transform _playerTransform;
    private Rigidbody2D _playerRigidbody;
    private Transform _weaponPivotTransform;
    private Player _player;
    public void Special()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _player = _playerTransform.GetComponent<PlayerController>().Player;
            _playerRigidbody = _playerTransform.GetComponent<Rigidbody2D>();
            _weaponPivotTransform = _playerTransform.transform.GetChild(0).GetChild(0);
        }
        if (_weapon == null)
        {
            _weapon = ResourceManager.Instance.LoadPrefab<GameObject>("Items/TestWeapon", "TestWeapon");
            _weapon = GameObject.Instantiate(_weapon, _weaponPivotTransform);
        }
        _weapon.SetActive(true);
        Vector2 dir = (_player.IsRight) ? Vector2.right : Vector2.left;
        _playerRigidbody.AddForce(dir * _player.MoveSpeed * 2, ForceMode2D.Impulse);
    }
}
