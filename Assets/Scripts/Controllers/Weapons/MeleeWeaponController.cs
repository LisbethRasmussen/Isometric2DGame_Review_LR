using System.Linq;
using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    protected override void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_contactPoint.position, _weaponData.Range);
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
