using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController
{
    [Header("Pathfinding")]
    [SerializeField] private Transform _target;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Vector2[] _patrolPoints;

    [Header("State Machine")]
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _stateIndicator;
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _fieldOfView;
    [SerializeField] private float _attackRange;

    [Header("Attack")]
    [SerializeField] private WeaponController _weaponController;

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
    public NavMeshAgent Agent => _agent;
    public Vector2[] PatrolPoints => _patrolPoints;

    public Animator Animator => _animator;
    public SpriteRenderer StateIndicator => _stateIndicator;
    public float DetectionRange => _detectionRange;
    public float FieldOfView => _fieldOfView;
    public float AttackRange => _attackRange;

    public WeaponController WeaponController => _weaponController;

    public EnemyIdleState IdleState => _idleState;
    public EnemyPatrolState PatrolState => _patrolState;
    public EnemyChaseState ChaseState => _chaseState;
    public EnemyAttackState AttackState => _attackState;

    private void Start()
    {
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _idleState = new EnemyIdleState(this);
        _patrolState = new EnemyPatrolState(this);
        _chaseState = new EnemyChaseState(this);
        _attackState = new EnemyAttackState(this);

        SwitchState(_idleState);

        if (_patrolPoints == null || _patrolPoints.Length < 1)
        {
            _patrolPoints = GameManager.Instance.DefaultPatrolPoints;
        }
    }

    protected override void Update()
    {
        base.Update();

        _currentState.UpdateState();
    }

    public void Setup(Transform target)
    {
        _target = target;
    }

    public void SwitchState(EnemyBaseState _baseState)
    {
        _currentState = _baseState;
        _currentState.EnterState();
    }

    protected override void HandleInput()
    {
        _currentState.HandleInput();
    }

    protected override void HandleAnimation()
    {
        _currentState.HandleAnimation();
    }

    public bool IsTargetVisible()
    {
        Vector2 direction = _target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, GameManager.Instance.ObstacleLayer);
        Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0f));
        return !hit;
    }
}