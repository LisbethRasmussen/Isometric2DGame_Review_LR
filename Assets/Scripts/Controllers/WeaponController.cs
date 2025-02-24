using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] protected Transform _contactPoint;
    [SerializeField] protected WeaponData _weaponData;

    protected Vector2 _attackDirection;

    public Vector2 AttackDirection
    {
        get => _attackDirection;
        set => _attackDirection = value;
    }

    public abstract void Attack();
}
