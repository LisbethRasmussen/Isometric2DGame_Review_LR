using UnityEngine;

public class EnemyController : IsometricController
{
    [Header("Pathfinding")]
    [SerializeField] private Transform _target;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private EnemyBaseState _currentState;
    private EnemyIdleState _idleState;
    private EnemyPatrolState _patrolState;
    private EnemyChaseState _chaseState;
    private EnemyAttackState _attackState;

    // Isometric Controller Variables
    public Rigidbody2D Rb => _rb;
    public float MoveSpeed => _moveSpeed;
    public Vector2 MoveDirection
    {
        get => _moveDirection;
        set => _moveDirection = value;
    }

    // Local Variables
    public Transform Target => _target;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public EnemyIdleState IdleState => _idleState;
    public EnemyPatrolState PatrolState => _patrolState;
    public EnemyChaseState ChaseState => _chaseState;
    public EnemyAttackState AttackState => _attackState;

    private void Start()
    {
        _idleState = new EnemyIdleState(this);
        _patrolState = new EnemyPatrolState(this);
        _chaseState = new EnemyChaseState(this);
        _attackState = new EnemyAttackState(this);

        SwitchState(_idleState);
    }

    public void Setup(Transform target)
    {
        _target = target;
    }

    public void SwitchState(EnemyBaseState _baseState)
    {
        _currentState = _baseState;
    }

    protected override void HandleInput()
    {
        _currentState.HandleInput();
    }

    protected override void HandleAnimation()
    {
        _currentState.HandleAnimation();
    }
}
