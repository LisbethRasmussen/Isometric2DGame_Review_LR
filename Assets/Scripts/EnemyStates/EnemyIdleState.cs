using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float _changeDirectionTime;
    private float _startPatrolTime;
    private int _faceDirection;

    public EnemyIdleState(EnemyController enemyController) : base(enemyController)
    {

    }

    public override void EnterState()
    {
        _changeDirectionTime = Random.Range(1f, 3f);
        _startPatrolTime = Random.Range(5f, 10f);
        _faceDirection = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
        _enemyController.ChangeFacing(_faceDirection);

        _enemyController.Animator.SetInteger("State", 0);
    }

    public override void UpdateState()
    {
        _startPatrolTime -= Time.deltaTime;
        if (_startPatrolTime <= 0f)
        {
            _enemyController.SwitchState(_enemyController.PatrolState);
        }

        Vector2 direction = _enemyController.Target.position - _enemyController.transform.position;
        if (direction.magnitude <= _enemyController.DetectionRange && _enemyController.IsTargetVisible())
        {
            Vector2 lookDirection = new Vector2(_faceDirection, 0f);
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
        _enemyController.StateIndicator.flipX = _enemyController.transform.localScale.x < 0f;
    }
}
