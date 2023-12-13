using UnityEngine;

public class MeleeCatch : ICatchBehaviour
{
    private GameObject _weapon;
    private Transform _playerTransform;
    private Transform _weaponPivotTransform;
    public void Catch()
    {
        if(_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _weaponPivotTransform = _playerTransform.transform.GetChild(0).GetChild(0);
        }
        if (_weapon == null)
        {
            _weapon = ResourceManager.Instance.LoadPrefab<GameObject>("Items/TestWeapon", "TestWeapon");
            _weapon = GameObject.Instantiate(_weapon, _weaponPivotTransform);
        }
        _weapon.SetActive(true);
    }
}
