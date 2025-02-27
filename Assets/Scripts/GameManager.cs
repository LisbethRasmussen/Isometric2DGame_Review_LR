using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Entities")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector2[] _defaultPatrolPoints;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private LayerMask _entityLayer;

    [Header("Particles")]
    [SerializeField] private GameObject _meleeAttackParticlePrefab;
    [SerializeField] private GameObject _rangedAttackParticlePrefab;
    [SerializeField] private GameObject _bloodParticlePrefab;
    [SerializeField] private GameObject _deathParticlePrefab;

    [Header("Enemy Spawner")]
    [SerializeField] private Vector2[] _mapBounds;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Vector2 _enemySpawnTime;
    [SerializeField] private float _minimumPlayerDistance;

    private bool _isGameOver;
    private float _enemySpawnCooldown;

    #region Variables Getters
    public Transform PlayerTransform => _playerTransform;
    public Vector2[] DefaultPatrolPoints => _defaultPatrolPoints;
    public LayerMask ObstacleLayer => _obstacleLayer;
    public LayerMask EntityLayer => _entityLayer;

    public GameObject MeleeAttackParticlePrefab => _meleeAttackParticlePrefab;
    public GameObject RangedAttackParticlePrefab => _rangedAttackParticlePrefab;
    public GameObject BloodParticlePrefab => _bloodParticlePrefab;
    public GameObject DeathParticlePrefab => _deathParticlePrefab;
    #endregion

    private void Start()
    {
        SetupGame();
    }

    private void Update()
    {
        if (!_isGameOver)
        {
            SpawnEnemies();
        }
    }

    private void SetupGame()
    {
        _isGameOver = false;
        _enemySpawnCooldown = Random.Range(_enemySpawnTime.x, _enemySpawnTime.y);
    }

    private void SpawnEnemies()
    {
        if (_enemySpawnCooldown <= 0)
        {
            Vector2 spawnPoint = Vector2.zero;
            for (int i = 0; i < 20; i++)
            {
                spawnPoint = new Vector2(Random.Range(_mapBounds[0].x, _mapBounds[1].x), Random.Range(_mapBounds[0].y, _mapBounds[1].y));
                float distance = Vector2.Distance(spawnPoint, _playerTransform.position);
                if (!Physics2D.OverlapCircle(spawnPoint, 1f, _obstacleLayer) && distance > _minimumPlayerDistance)
                {
                    break;
                }
            }
            GameObject enemyGO = Instantiate(_enemyPrefabs.SelectRandom(), spawnPoint, Quaternion.identity, _enemyTransform);
            enemyGO.GetComponent<EnemyController>().Setup(_playerTransform);

            _enemySpawnCooldown = Random.Range(_enemySpawnTime.x, _enemySpawnTime.y);
        }
        else
        {
            _enemySpawnCooldown -= Time.deltaTime;
        }

        Debug.DrawLine(_mapBounds[0], new Vector2(_mapBounds[0].x, _mapBounds[1].y), Color.white);
        Debug.DrawLine(_mapBounds[0], new Vector2(_mapBounds[1].x, _mapBounds[0].y), Color.white);
        Debug.DrawLine(_mapBounds[1], new Vector2(_mapBounds[0].x, _mapBounds[1].y), Color.white);
        Debug.DrawLine(_mapBounds[1], new Vector2(_mapBounds[1].x, _mapBounds[0].y), Color.white);
        ExtensionMethods.DrawEllipse(_playerTransform.position, _minimumPlayerDistance, _minimumPlayerDistance, Color.white);
    }

    public void EndGame()
    {
        _isGameOver = true;
        MenuManager.Instance.OpenEndScreen();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        SetupGame();
    }

    private void OnDrawGizmos()
    {
        if (_defaultPatrolPoints != null)
        {
            foreach (Vector2 patrolPoint in _defaultPatrolPoints)
            {
                Gizmos.DrawSphere(patrolPoint, 0.5f);
            }
        }
    }
}
