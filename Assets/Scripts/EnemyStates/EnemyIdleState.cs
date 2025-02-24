using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float _changeDirectionTime;
    private float _startPatrolTime;

    public EnemyIdleState(EnemyController enemyController) : base(enemyController)
    {

    }

    public override void EnterState()
    {
        _changeDirectionTime = Random.Range(1f, 3f);
        _startPatrolTime = Random.Range(5f, 10f);
    }

    public override void UpdateState()
    {
        _startPatrolTime -= Time.deltaTime;
        if (_startPatrolTime <= 0)
        {
            _enemyController.SwitchState(_enemyController.PatrolState);
        }

        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude <= _enemyController.DetectionRange)
        {
            Vector2 lookDirection = new Vector2(_enemyController.SpriteRenderer.flipX ? -1 : 1, 0);
            float angle = Vector2.Angle(lookDirection, direction);
            if (angle < _enemyController.FieldOfView / 2f)
            {
                _enemyController.SwitchState(_enemyController.ChaseState);
            }
        }
    }

    public override void HandleInput()
    {
        _enemyController.MoveDirection = Vector2.zero;
    }

    public override void HandleAnimation()
    {
        if (_changeDirectionTime <= 0)
        {
            _enemyController.SpriteRenderer.flipX = !_enemyController.SpriteRenderer.flipX;
            _changeDirectionTime = Random.Range(2f, 5f);
        }
        else
        {
            _changeDirectionTime -= Time.deltaTime;
        }
    }
}
