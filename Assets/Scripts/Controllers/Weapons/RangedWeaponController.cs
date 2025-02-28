using UnityEngine;

/// <summary>
/// Handles ranged weapon behaviour against rival entities.
/// </summary>
public class RangedWeaponController : WeaponController
{
    [SerializeField] private GameObject _projectilePrefab;

    /// <summary>
    /// Initializes and shoots a projectile to damage rival entities.
    /// </summary>
    protected override void Attack()
    {
        // Calculate an offset, so the projectile doesn't spawn inside of the entity
        Vector3 spawnOffset = new Vector3(_attackDirection.normalized.x, _attackDirection.normalized.y, 0f) * _projectilePrefab.transform.localScale.x / 2f;

        // Initialize and pass necessary variables to the projectile object
        GameObject projectileGO = Instantiate(_projectilePrefab, ContactPoint.position + spawnOffset, Quaternion.identity);
        projectileGO.GetComponent<ProjectileController>().Setup(_attackDirection, WeaponData);

        // Create a ranged attack particle for visual cues
        GameObject particleGO = Instantiate(GameManager.Instance.RangedAttackParticlePrefab, ContactPoint.position, Quaternion.identity, ContactPoint);
        Destroy(particleGO, 1f);
    }
}
