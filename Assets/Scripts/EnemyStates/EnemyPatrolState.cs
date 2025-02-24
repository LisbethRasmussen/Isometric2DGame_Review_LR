using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyController enemyController) : base(enemyController)
    {
        
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude <= _enemyController.DetectionRange)
        {
            float angle = Vector2.Angle(_enemyController.MoveDirection, direction);
            if (angle < _enemyController.FieldOfView / 2f)
            {
                _enemyController.SwitchState(_enemyController.ChaseState);
            }
        }
    }

    public override void HandleInput()
    {
        
    }

    public override void HandleAnimation()
    {
        if (_enemyController.MoveDirection.x != 0)
        {
            _enemyController.SpriteRenderer.flipX = _enemyController.MoveDirection.x < 0;
        }
    }
}
