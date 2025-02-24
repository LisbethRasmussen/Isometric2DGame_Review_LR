using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] protected Transform _contactPoint;
    [SerializeField] protected WeaponData _weaponData;

    protected Vector2 _attackDirection;

    private float _attackCooldown;

    public Transform ContactPoint => _contactPoint;
    public Vector2 AttackDirection
    {
        get => _attackDirection;
        set => _attackDirection = value;
    }

    private void Update()
    {
        if (_attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }

    public void HandleAttack()
    {
        if (_attackCooldown <= 0f)
        {
            Attack();
            _attackCooldown = 1f / _weaponData.AttackRate;
        }
    }

    protected abstract void Attack();
}
