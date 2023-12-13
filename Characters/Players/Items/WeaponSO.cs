using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "DataSO/Item/Weapon", order = 1)]
public class WeaponSO : ItemSO
{
    [Header("Weapon")]
    public float Attack;
    public float AttackSpeed;
    public GameObject Projectile;
}
