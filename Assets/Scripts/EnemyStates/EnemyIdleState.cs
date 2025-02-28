using UnityEngine;

/// <summary>
/// Handles idling logic for the enemy.
/// </summary>
public class EnemyIdleState : EnemyBaseState
{
    private float _changeDirectionTime;
    private float _startPatrolTime;
    private int _faceDirection;

    public EnemyIdleState(EnemyController enemyController) : base(enemyController)
    {

    }

    /// <summary>
    /// Initializes the timers and other variables for the idle state.
    /// </summary>
    public override void EnterState()
    {
        _changeDirectionTime = Random.Range(1f, 3f);
        _startPatrolTime = Random.Range(5f, 10f);
        _faceDirection = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
        _enemyController.ChangeFacing(_faceDirection);

        _enemyController.Animator.SetInteger("State", 0);
    }

    /// <summary>
    /// Checks if the enemy should start patrolling or the target is visible and it should start chasing it..
    /// </summary>
    public override void UpdateState()
    {
        // Update the timer and enter patrol state if it has ended
        _startPatrolTime -= Time.deltaTime;
        if (_startPatrolTime <= 0f)
        {
            _enemyController.SwitchState(_enemyController.PatrolState);
        }

        // Check if the target is visible and is in the detection range
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude <= _enemyController.DetectionRange && _enemyController.IsTargetVisible())
        {
            // Check if the target is visible in the field of view of the enemy, before entering the chase state
            Vector2 lookDirection = new Vector2(_faceDirection, 0f);
            float angle = Vector2.Angle(lookDirection, direction);
            if (angle < _enemyController.FieldOfView / 2f)
            {
                _enemyController.SwitchState(_enemyController.ChaseState);
            }
        }
    }

    /// <summary>
    /// Freezes the enemy in place, while in the idle state.
    /// </summary>
    public override void HandleInput()
    {
        _enemyController.MoveDirection = Vector2.zero;
    }

    /// <summary>
    /// Changes the direction the enemy faces every fwe seconds.
    /// </summary>
    public override void HandleAnimation()
    {
        // Change the direction the enemy faces if the timer runs out and restart it
        if (_changeDirectionTime <= 0f)
        {
            _faceDirection *= -1;
            _enemyController.ChangeFacing(_faceDirection);
            _changeDirectionTime = Random.Range(1f, 3f);
        }
        else
        {
            _changeDirectionTime -= Time.deltaTime;
        }
    }
}
