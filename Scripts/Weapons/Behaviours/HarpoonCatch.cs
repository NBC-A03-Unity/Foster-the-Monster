using UnityEngine;

public class HarpoonCatch : ISpecialBehaviour
{
    private const string _prefabName = "TestProjectile";
    private GameObject _projectile;
    public void Special()
    {
        if (_projectile == null || _projectile == ResourceManager.Instance.LoadPrefab<GameObject>("Items/TestProjectile", "TestProjectile"))
        {
            _projectile = ResourceManager.Instance.LoadPrefab<GameObject>("Items/TestHarpoon", "TestHarpoon"); ;
            _projectile = GameObject.Instantiate(_projectile);
        }
        _projectile.SetActive(true);
    }
}
