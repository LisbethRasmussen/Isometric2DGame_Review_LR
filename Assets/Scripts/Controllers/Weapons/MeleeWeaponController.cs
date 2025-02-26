using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    protected override void Attack()
    {
        Vector2 attackOffset = _attackDirection.normalized * _weaponData.Range / 2f;
        Vector2 attackPoint = new Vector2(_contactPoint.position.x, _contactPoint.position.y) + attackOffset;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint, _weaponData.Range, GameManager.Instance.EntityLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out EntityController entityController))
            {
                if (entityController.EntityData.Team != _weaponData.Entity.EntityData.Team)
                {
                    entityController.TakeDamage(_weaponData.Damage);
                }
            }
        }
    }
}
