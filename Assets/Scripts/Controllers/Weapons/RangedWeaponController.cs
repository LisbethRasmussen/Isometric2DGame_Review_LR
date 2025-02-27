using UnityEngine;

public class RangedWeaponController : WeaponController
{
    [SerializeField] private GameObject _projectilePrefab;

    protected override void Attack()
    {
        Vector3 spawnOffset = new Vector3(_attackDirection.normalized.x, _attackDirection.normalized.y, 0f) * _projectilePrefab.transform.localScale.x / 2f;
        GameObject projectileGO = Instantiate(_projectilePrefab, ContactPoint.position + spawnOffset, Quaternion.identity);
        projectileGO.GetComponent<ProjectileController>().Setup(_attackDirection, WeaponData);

        GameObject particleGO = Instantiate(GameManager.Instance.RangedAttackParticlePrefab, ContactPoint.position, Quaternion.identity, ContactPoint);
        Destroy(particleGO, 1f);
    }
}
