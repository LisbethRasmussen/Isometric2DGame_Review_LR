using UnityEngine;

/// <summary>
/// Serves as a base class for weapons used by entities.
/// Should be extended by weapons that use a cooldown system.
/// </summary>
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
        // If the weapon is on cooldown, decrease the wait time
        if (_attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Performs an attack action if the weapon is ready and resets its cooldown.
    /// </summary>
    public void HandleAttack()
    {
        // If the weapon is ready, call the attack function and restart the cooldown
        if (_attackCooldown <= 0f)
        {
            Attack();
            _attackCooldown = 1f / _weaponData.AttackRate;
        }
    }

    /// <summary>
    /// Executes logic when the weapon is ready to used.
    /// Should be implemented to handle collision or initialization logic for the action.
    /// </summary>
    protected abstract void Attack();
}
