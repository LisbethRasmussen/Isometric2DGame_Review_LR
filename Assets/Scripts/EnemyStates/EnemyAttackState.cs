using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyController enemyController) : base(enemyController)
    {

    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude > _enemyController.AttackRange)
        {
            _enemyController.SwitchState(_enemyController.ChaseState);
        }
    }

    public override void HandleInput()
    {
        _enemyController.MoveDirection = Vector2.zero;
    }

    public override void HandleAnimation()
    {
        float direction = _enemyController.Target.position.x - _enemyController.transform.position.x;
        _enemyController.SpriteRenderer.flipX = direction < 0f;
    }
}
