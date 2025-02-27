using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float _abilityCooldown;
    private float _attackRate;

    public EnemyAttackState(EnemyController enemyController) : base(enemyController)
    {
        _attackRate = _enemyController.WeaponController.WeaponData.AttackRate;
    }

    public override void EnterState()
    {
        _enemyController.Animator.SetInteger("State", 3);

        _abilityCooldown = _enemyController.AbilityCooldown;
    }

    public override void UpdateState()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude > _enemyController.AttackRange * 1.5f || !_enemyController.IsTargetVisible())
        {
            _enemyController.SwitchState(_enemyController.ChaseState);
        }
        else if (_enemyController.Target.TryGetComponent(out EntityController entityController))
        {
            if (entityController.EntityData.Health <= 0)
            {
                _enemyController.SwitchState(_enemyController.IdleState);
            }
        }

        Vector2 attackDirection = _enemyController.Target.position - _enemyController.WeaponController.ContactPoint.position;
        _enemyController.WeaponController.AttackDirection = attackDirection;
        _enemyController.WeaponController.HandleAttack();

        HandleAbility();
    }

    public override void HandleInput()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        _enemyController.MoveDirection = _enemyController.WeaponController is MeleeWeaponController ? direction : Vector2.zero;
    }

    public override void HandleAnimation()
    {
        float direction = _enemyController.Target.position.x - _enemyController.transform.position.x;
        _enemyController.ChangeFacing(direction);
    }

    private void HandleAbility()
    {
        _abilityCooldown -= Time.deltaTime;
        if (_abilityCooldown <= 0)
        {
            _enemyController.WeaponController.WeaponData.AttackRate = _attackRate * 4f;
            if (_abilityCooldown <= -_enemyController.AbilityDuration)
            {
                _abilityCooldown = _enemyController.AbilityCooldown;
                _enemyController.WeaponController.WeaponData.AttackRate = _attackRate;
            }
        }
    }
}
