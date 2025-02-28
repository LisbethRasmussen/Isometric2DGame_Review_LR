using UnityEngine;

/// <summary>
/// Handles logic for chasing the target.
/// </summary>
public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyController enemyController) : base(enemyController)
    {

    }

    /// <summary>
    /// Updates the animation of the enemy.
    /// </summary>
    public override void EnterState()
    {
        _enemyController.Animator.SetInteger("State", 2);
    }

    /// <summary>
    /// Chases the target using pathfinding and checks if it's close enough to be attacked or returns to idle state if it escapes.
    /// </summary>
    public override void UpdateState()
    {
        // Check if the target is visible and is in atack range, before entering the attack state
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        float distance = direction.magnitude;
        if (distance <= _enemyController.AttackRange && _enemyController.IsTargetVisible())
        {
            _enemyController.SwitchState(_enemyController.AttackState);
        }
        // Check if the target is outside the detection range or dead, before returning to idle state
        else if (distance > _enemyController.DetectionRange * 1.5f)
        {
            _enemyController.SwitchState(_enemyController.IdleState);
        } 
        else if (_enemyController.Target.TryGetComponent(out EntityController entityController))
        {
            if (entityController.EntityData.Health <= 0)
            {
                _enemyController.SwitchState(_enemyController.IdleState);
            }
        }
    }

    /// <summary>
    /// Handles pathfinding logic to follow the target.
    /// </summary>
    public override void HandleInput()
    {
        // Calculate the path towards the target using Unity's NavMesh system and start moving
        _enemyController.Agent.nextPosition = _enemyController.transform.position;
        _enemyController.Agent.SetDestination(_enemyController.Target.position);
        _enemyController.MoveDirection = _enemyController.Agent.desiredVelocity;
    }

    /// <summary>
    /// Makes the enemy face towards the target.
    /// </summary>
    public override void HandleAnimation()
    {
        float direction = _enemyController.Target.position.x - _enemyController.transform.position.x;
        _enemyController.ChangeFacing(direction);
    }
}
