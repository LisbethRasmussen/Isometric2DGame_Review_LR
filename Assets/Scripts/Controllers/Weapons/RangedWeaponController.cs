using UnityEngine;

public class RangedWeaponController : WeaponController
{
    [SerializeField] private GameObject _projectilePrefab;

    protected override void Attack()
    {
        Vector3 spawnOffset = new Vector3(_attackDirection.normalized.x, _attackDirection.normalized.y, 0f) * _projectilePrefab.transform.localScale.x / 2f;
        GameObject projectileGO = Instantiate(_projectilePrefab, _contactPoint.position + spawnOffset, Quaternion.identity);
        projectileGO.GetComponent<ProjectileController>().Setup(_attackDirection, _weaponData);
    }
}
