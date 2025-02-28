using UnityEngine;

/// <summary>
/// Handles melee weapon behaviour against rival entities.
/// </summary>
public class MeleeWeaponController : WeaponController
{
    /// <summary>
    /// Damages every rival entities within the range of the weapon.
    /// </summary>
    protected override void Attack()
    {
        // Calculate the center point of the attack
        Vector2 attackOffset = _attackDirection.normalized * WeaponData.Range / 2f;
        Vector2 attackPoint = new Vector2(ContactPoint.position.x, ContactPoint.position.y) + attackOffset;

        // Find all colliders within the attack range, check which of them are entities of a different team and damage them
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint, WeaponData.Range, GameManager.Instance.EntityLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out EntityController entityController))
            {
                if (entityController.EntityData.Team != WeaponData.Entity.EntityData.Team)
                {
                    Vector2 contactPoint = (ContactPoint.position + collider.transform.position) / 2f;
                    entityController.TakeDamage(WeaponData.Damage, contactPoint);
                }
            }
        }

        // Create a melee attack particle for visual cues
        GameObject particleGO = Instantiate(GameManager.Instance.MeleeAttackParticlePrefab, ContactPoint.position, Quaternion.identity, ContactPoint);
        Destroy(particleGO, 1f);
    }
}
