using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    protected override void Attack()
    {
        Vector2 attackOffset = _attackDirection.normalized * WeaponData.Range / 2f;
        Vector2 attackPoint = new Vector2(ContactPoint.position.x, ContactPoint.position.y) + attackOffset;
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

        GameObject particleGO = Instantiate(GameManager.Instance.MeleeAttackParticlePrefab, ContactPoint.position, Quaternion.identity, ContactPoint);
        Destroy(particleGO, 1f);
    }
}
