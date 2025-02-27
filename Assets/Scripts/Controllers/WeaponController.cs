using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform _contactPoint;
    [SerializeField] private WeaponData _weaponData;

    protected Vector2 _attackDirection;

    private float _attackCooldown;

    #region Variable Getters
    public Transform ContactPoint => _contactPoint;
    public WeaponData WeaponData => _weaponData;
    public Vector2 AttackDirection
    {
        get => _attackDirection;
        set => _attackDirection = value;
    }
    #endregion

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
