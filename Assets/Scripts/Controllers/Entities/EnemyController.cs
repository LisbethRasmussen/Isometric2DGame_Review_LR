using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls enemy behaviour against the player.
/// </summary>
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
    [SerializeField] private float _abilityCooldown;
    [SerializeField] private float _abilityDuration;

    private EnemyBaseState _currentState;
    private EnemyIdleState _idleState;
    private EnemyPatrolState _patrolState;
    private EnemyChaseState _chaseState;
    private EnemyAttackState _attackState;

    #region Variable Getters
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
    public float AbilityCooldown => _abilityCooldown;
    public float AbilityDuration => _abilityDuration;

    public EnemyIdleState IdleState => _idleState;
    public EnemyPatrolState PatrolState => _patrolState;
    public EnemyChaseState ChaseState => _chaseState;
    public EnemyAttackState AttackState => _attackState;
    #endregion

    protected override void Start()
    {
        base.Start();

        // Disable NavMesh agent, as it isn't used for moving the enemy
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        // Initialize the state machine states and switch the idle as the default state
        _idleState = new EnemyIdleState(this);
        _patrolState = new EnemyPatrolState(this);
        _chaseState = new EnemyChaseState(this);
        _attackState = new EnemyAttackState(this);

        SwitchState(_idleState);

        // If there are no patrol points defined, use the default ones
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

    /// <summary>
    /// Passes parameters to the enemy after initialization.
    /// </summary>
    /// <param name="target">Reference to the target.</param>
    public void Setup(Transform target)
    {
        _target = target;
    }

    /// <summary>
    /// Handles the input of the current state of the enemy.
    /// </summary>
    protected override void HandleInput()
    {
        _currentState.HandleInput();
    }

    /// <summary>
    /// Handles the animation of the current state of the enemy.
    /// </summary>
    protected override void HandleAnimation()
    {
        _currentState.HandleAnimation();

        // Flip the sprite of the state indicator to match it to the entity's scale, so it doesn't look flipped
        StateIndicator.flipX = transform.localScale.x < 0f;
    }

    /// <summary>
    /// Runs logic to indicate the death of the enemy.
    /// </summary>
    protected override void HandleDeath()
    {
        // Create a death particle for visual cues
        GameObject particleGO = Instantiate(GameManager.Instance.DeathParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particleGO, 1f);

        Destroy(gameObject);
    }

    /// <summary>
    /// Changes the state of the enemy's state machine.
    /// </summary>
    /// <param name="baseState">The state the state machine should switch to.</param>
    public void SwitchState(EnemyBaseState baseState)
    {
        _currentState = baseState;
        _currentState.EnterState();
    }

    /// <summary>
    /// Checks if anything is blocking the line of sight between the enemy and its target.
    /// </summary>
    /// <returns>A bool for indicating if the target is visible.</returns>
    public bool IsTargetVisible()
    {
        // If the target is not active, consider it not visible
        if (!_target.gameObject.activeSelf)
        {
            return false;
        }

        // Shoot a ray in the direction of the target, only check for hits on the obstacle layer and visualize and return the result
        Vector2 direction = _target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, GameManager.Instance.ObstacleLayer);
        Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0f), hit ? Color.red : Color.white);
        return !hit;
    }
}