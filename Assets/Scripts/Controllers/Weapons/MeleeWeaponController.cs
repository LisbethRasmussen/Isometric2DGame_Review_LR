using System.Linq;
using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    public override void Attack()
    {
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(_contactPoint.position, _weaponData.Range);
        Debug.Log(string.Join(",", objectsHit.Select(o => o.transform.name)));
    }
}
