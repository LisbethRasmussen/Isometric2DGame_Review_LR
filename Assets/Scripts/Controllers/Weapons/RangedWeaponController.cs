using UnityEngine;

public class RangedWeaponController : WeaponController
{
    [SerializeField] private GameObject _projectilePrefab;

    public override void Attack()
    {
        GameObject projectileGO = Instantiate(_projectilePrefab, _contactPoint.position, Quaternion.identity);
        projectileGO.GetComponent<ProjectileController>().Setup(_attackDirection, _weaponData);
    }
}
