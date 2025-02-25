using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private int _currentPointIndex;
    private float _standTime;
    private float _skipPointTime;
    private Vector2[] _currentPath;

    public EnemyPatrolState(EnemyController enemyController) : base(enemyController)
    {
        
    }

    public override void EnterState()
    {
        _currentPointIndex = 0;
        _standTime = 0f;
        _skipPointTime = 0f;
        _currentPath = GetRandomPath();

        _enemyController.Animator.SetInteger("State", 1);
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
        if (Vector2.Distance(currentPoint, _enemyController.transform.position) <= _enemyController.MoveSpeed * Time.fixedDeltaTime || _skipPointTime <= 0)
        {
            _currentPointIndex++;
            _standTime = Random.Range(0f, 1f);
            _skipPointTime = 10f;
        }

        if (_standTime <= 0)
        {
            _enemyController.Agent.nextPosition = _enemyController.transform.position;
            _enemyController.Agent.SetDestination(currentPoint);
            _enemyController.MoveDirection = _enemyController.Agent.desiredVelocity;
        }
        else
        {
            _standTime -= Time.deltaTime;
            _enemyController.MoveDirection = Vector2.zero;
        }

        if (_skipPointTime > 0)
        {
            _skipPointTime -= Time.deltaTime;
        }
    }

    public override void HandleAnimation()
    {
        if (_enemyController.MoveDirection.x != 0f)
        {
            _enemyController.ChangeFacing(_enemyController.MoveDirection.x);
        }
        _enemyController.StateIndicator.flipX = _enemyController.transform.localScale.x < 0f;
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
