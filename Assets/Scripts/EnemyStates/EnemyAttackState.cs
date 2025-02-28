using UnityEngine;

/// <summary>
/// Handles logic for attacking the target.
/// </summary>
public class EnemyAttackState : EnemyBaseState
{
    private float _abilityCooldown;
    private float _attackRate;

    public EnemyAttackState(EnemyController enemyController) : base(enemyController)
    {
        _attackRate = _enemyController.WeaponController.WeaponData.AttackRate;
    }

    /// <summary>
    /// Initializes variables and updates the animation of the enemy.
    /// </summary>
    public override void EnterState()
    {
        _enemyController.Animator.SetInteger("State", 3);

        _abilityCooldown = _enemyController.AbilityCooldown;
    }

    /// <summary>
    /// Performs the attacking action and checks if the target is too far to be attacked or dead.
    /// </summary>
    public override void UpdateState()
    {
        // Check if the target is outside the attacking range, before switching to chase state
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude > _enemyController.AttackRange * 1.5f || !_enemyController.IsTargetVisible())
        {
            _enemyController.SwitchState(_enemyController.ChaseState);
        }
        // Checks if the target is dead, before returning to idle state
        else if (_enemyController.Target.TryGetComponent(out EntityController entityController))
        {
            if (entityController.EntityData.Health <= 0)
            {
                _enemyController.SwitchState(_enemyController.IdleState);
            }
        }

        // Calculate the parameters of the attack and perform the action
        Vector2 attackDirection = _enemyController.Target.position - _enemyController.WeaponController.ContactPoint.position;
        _enemyController.WeaponController.AttackDirection = attackDirection;
        _enemyController.WeaponController.HandleAttack();

        HandleAbility();
    }

    /// <summary>
    /// Handles logic for following the target or standing still based on the weapon used.
    /// </summary>
    public override void HandleInput()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        _enemyController.MoveDirection = _enemyController.WeaponController is MeleeWeaponController ? direction : Vector2.zero;
    }

    /// <summary>
    /// Makes the enemy face towards the target.
    /// </summary>
    public override void HandleAnimation()
    {
        float direction = _enemyController.Target.position.x - _enemyController.transform.position.x;
        _enemyController.ChangeFacing(direction);
    }

    /// <summary>
    /// Performs a special ability every few seconds.
    /// </summary>
    private void HandleAbility()
    {
        // Update the ability timer and increase the attack rate for a few seconds
        _abilityCooldown -= Time.deltaTime;
        if (_abilityCooldown <= 0)
        {
            _enemyController.WeaponController.WeaponData.AttackRate = _attackRate * 4f;
            // After the ability runs out, reset the timer and change back the attack rate
            if (_abilityCooldown <= -_enemyController.AbilityDuration)
            {
                _abilityCooldown = _enemyController.AbilityCooldown;
                _enemyController.WeaponController.WeaponData.AttackRate = _attackRate;
            }
        }
    }
}
