using System.Linq;
using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    protected override void Attack()
    {
        Vector2 attackPoint = new Vector2(_contactPoint.position.x, _contactPoint.position.y) + _attackDirection * _weaponData.Range;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint, _weaponData.Range);
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
