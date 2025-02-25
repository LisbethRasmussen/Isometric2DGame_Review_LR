using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyController enemyController) : base(enemyController)
    {

    }

    public override void EnterState()
    {
        _enemyController.Animator.SetInteger("State", 2);
    }

    public override void UpdateState()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        float distance = direction.magnitude;
        if (distance <= _enemyController.AttackRange)
        {
            _enemyController.SwitchState(_enemyController.AttackState);
        }
        else if (distance > _enemyController.DetectionRange * 2f)
        {
            _enemyController.SwitchState(_enemyController.IdleState);
        }
    }

    public override void HandleInput()
    {
        _enemyController.Agent.nextPosition = _enemyController.transform.position;
        _enemyController.Agent.SetDestination(_enemyController.Target.position);
        _enemyController.MoveDirection = _enemyController.Agent.desiredVelocity;
    }

    public override void HandleAnimation()
    {
        float direction = _enemyController.Target.position.x - _enemyController.transform.position.x;
        _enemyController.ChangeFacing(direction);
        _enemyController.StateIndicator.flipX = _enemyController.transform.localScale.x < 0f;
    }
}
