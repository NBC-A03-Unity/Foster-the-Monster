using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

public class RangedCatch : ICatchBehaviour
{
    private AssetReferenceGameObject _projectilePrefabReference;
    private Projectile _projectilePrefab;
    public Queue<GameObject> Projectiles;
    private IObjectPool<Projectile> _projectilePool;

    public async void Catch()
    {
        if(_projectilePrefab == null)
        {
            _projectilePrefabReference = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ProjectilePrefab;
            _projectilePrefab = await ObjectManager.Instance.UsePool<Projectile>(_projectilePrefabReference);
            _projectilePrefab.ProjectilePool = _projectilePool;
        }

        if (_projectilePool == null)
        {
            _projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledProjectile, true, 15, 50);
        }

        Projectile projectile = _projectilePool.Get();

    }

    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = GameObject.Instantiate(_projectilePrefab);
        projectileInstance.ProjectilePool = _projectilePool;

        return projectileInstance;
    }

    private void OnGetFromPool(Projectile pooledProjectile)
    {
        pooledProjectile.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Projectile pooledProjectile)
    {
        pooledProjectile.gameObject.SetActive(false);
    }

    private void OnDestroyPooledProjectile(Projectile pooledProjectile)
    {
        GameObject.Destroy(pooledProjectile.gameObject);
    }

    
}
