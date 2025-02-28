using UnityEngine;

/// <summary>
/// Handles patrolling and pathfinding logic for the enemy.
/// </summary>
public class EnemyPatrolState : EnemyBaseState
{
    private int _currentPointIndex;
    private float _standTime;
    private float _skipPointTime;
    private Vector2[] _currentPath;

    public EnemyPatrolState(EnemyController enemyController) : base(enemyController)
    {
        
    }

    /// <summary>
    /// Inizializes the variables and path for the patrol state.
    /// </summary>
    public override void EnterState()
    {
        _currentPointIndex = 0;
        _standTime = 0f;
        _skipPointTime = 0f;
        _currentPath = GetRandomPath();

        _enemyController.Animator.SetInteger("State", 1);
    }

    /// <summary>
    /// Checks if the target is visible and should be chased or if it should return to the idle state.
    /// </summary>
    public override void UpdateState()
    {
        // Check if the target is visible and is in the detection range
        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude <= _enemyController.DetectionRange && _enemyController.IsTargetVisible())
        {
            // Check if the target is visible in the field of view of the enemy, before entering the chase state
            float angle = Vector2.Angle(_enemyController.MoveDirection, direction);
            if (angle < _enemyController.FieldOfView / 2f)
            {
                _enemyController.SwitchState(_enemyController.ChaseState);
            }
        }
        // After completing the patrol route, return to the idle state
        if (_currentPointIndex >= _currentPath.Length)
        {
            _enemyController.SwitchState(_enemyController.IdleState);
        }
    }

    /// <summary>
    /// Handles pathfinding logic for moving from one point of the patrol route to the next one.
    /// </summary>
    public override void HandleInput()
    {
        // If the patrol route is finished, return
        if (_currentPointIndex >= _currentPath.Length)
        {
            return;
        }

        // Check if the distance between the enemy and the target point is small enough or if it's been too long and move onto the next one point
        Vector2 currentPoint = _currentPath[_currentPointIndex];
        if (Vector2.Distance(currentPoint, _enemyController.transform.position) <= _enemyController.MoveSpeed * Time.fixedDeltaTime || _skipPointTime <= 0)
        {
            _currentPointIndex++;
            // Wait a bit, before moving on to the next point, to make the movement more natural
            _standTime = Random.Range(0f, 1f);
            _skipPointTime = 10f;
        }

        // If the enemy is ready to continute on its path, calculate the path towards the next point using Unity's NavMesh system and start moving
        if (_standTime <= 0)
        {
            _enemyController.Agent.nextPosition = _enemyController.transform.position;
            _enemyController.Agent.SetDestination(currentPoint);
            _enemyController.MoveDirection = _enemyController.Agent.desiredVelocity;
        }
        // Update the timer for waiting before moving on to the next point
        else
        {
            _standTime -= Time.deltaTime;
            _enemyController.MoveDirection = Vector2.zero;
        }

        // If the enemy took too much time to reach the next point, skip it to prevent it from getting stuck
        if (_skipPointTime > 0)
        {
            _skipPointTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Makes the enemy face towards the direction it's moving.
    /// </summary>
    public override void HandleAnimation()
    {
        // If the enemy is moving, change its facing towards the direction it's moving
        if (_enemyController.MoveDirection.x != 0f)
        {
            _enemyController.ChangeFacing(_enemyController.MoveDirection.x);
        }
    }

    /// <summary>
    /// Generates a random path with a random length, using the available patrol points of the enemy.
    /// </summary>
    /// <returns>An array of points used as a route for the enemy to move along.</returns>
    private Vector2[] GetRandomPath()
    {
        // Initialize the array for storing the path
        int pathLength = Random.Range(3, 10);
        Vector2[] path = new Vector2[pathLength];
        
        // Set the first point as random
        path[0] = _enemyController.PatrolPoints.SelectRandom();
        // Iterate over the rest of the array and try to find a new point in the available patrol points, that is different from the previous one
        for (int i = 1; i < path.Length; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Vector2 nextPoint = _enemyController.PatrolPoints.SelectRandom();
                if (nextPoint != path[i - 1])
                {
                    path[i] = nextPoint;
                    break;
                }
            }
        }
        return path;
    }
}
