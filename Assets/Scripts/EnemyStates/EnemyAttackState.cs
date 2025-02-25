using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyController enemyController) : base(enemyController)
    {

    }

    public override void EnterState()
    {
        _enemyController.Animator.SetInteger("State", 3);
    }

    public override void UpdateState()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude > _enemyController.AttackRange)
        {
            _enemyController.SwitchState(_enemyController.ChaseState);
        }

        Vector2 attackDirection = _enemyController.Target.position - _enemyController.WeaponController.ContactPoint.position;
        _enemyController.WeaponController.AttackDirection = attackDirection;
        _enemyController.WeaponController.HandleAttack();
    }

    public override void HandleInput()
    {
        _enemyController.MoveDirection = Vector2.zero;
    }

    public override void HandleAnimation()
    {
        float direction = _enemyController.Target.position.x - _enemyController.transform.position.x;
        _enemyController.ChangeFacing(direction);
        _enemyController.StateIndicator.flipX = _enemyController.transform.localScale.x < 0f;
    }
}
