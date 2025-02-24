using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyController enemyController) : base(enemyController)
    {

    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        float distance = direction.magnitude;
        if (distance <= _enemyController.AttackRange)
        {
            _enemyController.SwitchState(_enemyController.AttackState);
        }
        else if (distance > _enemyController.DetectionRange)
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
    }
}
