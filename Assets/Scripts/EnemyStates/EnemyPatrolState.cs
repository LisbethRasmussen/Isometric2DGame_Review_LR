using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private int _currentPointIndex;
    private Vector2[] _currentPath;

    public EnemyPatrolState(EnemyController enemyController) : base(enemyController)
    {
        
    }

    public override void EnterState()
    {
        _currentPointIndex = 0;
        _currentPath = GetRandomPath();
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
        if (_currentPointIndex >= _currentPath.Length)
        {
            _enemyController.SwitchState(_enemyController.IdleState);
        }
    }

    public override void HandleInput()
    {
        if (_currentPointIndex >= _currentPath.Length)
        {
            return;
        }

        Vector2 currentPoint = _currentPath[_currentPointIndex];
        if (Vector2.Distance(currentPoint, _enemyController.transform.position) <= _enemyController.MoveSpeed * Time.fixedDeltaTime)
        {
            _currentPointIndex++;
        }

        _enemyController.Agent.nextPosition = _enemyController.transform.position;
        _enemyController.Agent.SetDestination(currentPoint);
        _enemyController.MoveDirection = _enemyController.Agent.desiredVelocity;
    }

    public override void HandleAnimation()
    {
        if (_enemyController.MoveDirection.x != 0f)
        {
            _enemyController.ChangeFacing(_enemyController.MoveDirection.x);
        }
    }

    private Vector2[] GetRandomPath()
    {
        int pathLength = Random.Range(3, 10);
        Vector2[] path = new Vector2[pathLength];

        path[0] = _enemyController.PatrolPoints.SelectRandom();
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
