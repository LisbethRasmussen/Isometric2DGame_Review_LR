using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main class for managing the game loop.
/// </summary>
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
        //--- Lisbeth: Consider making a 'suspend methods' architecture instead of setting the
        // timescale to 0. This could be with a static bool isPaused, which can be checked by
        // the relevant running methods. Ideally, everything which is a supposed to use an
        // Update(), could be suspended in one place, if an UpdateManager was implemented.
        Time.timeScale = 0f;
        _playerTransform.gameObject.SetActive(false);

        _isGameOver = false;
        _enemySpawnCooldown = Random.Range(_enemySpawnTime.x, _enemySpawnTime.y);
    }

    private void Update()
    {
        if (!_isGameOver)
        {
            SpawnEnemies();
        }
    }

    /// <summary>
    /// Spawns enemies every few seconds within the given boundaries, ensuring they spawn on a valid point of the map.
    /// </summary>
    private void SpawnEnemies()
    {
        //--- Lisbeth: What I would have done:
        // Create a separate method/script for the drawing the lines, and cached the vectors instead of
        // using 'new'.
        // started the SpawnEnemies() method like this:
        /*
        if (_enemySpawnCooldown > 0)
        {
            _enemySpawnCooldown -= Time.deltaTime;
            return;
        }
        */
        // and then put in the rest of the logic below, since there is no reason for the system to read
        // through, and ignore the lines which are not supposed to be used while spawn time is above 0

        //--- Lisbeth: I will assume, that you normally would do a cap on how many active enemies
        // can be active at any give point in time, where you would not even run the cool down
        // logic for as long as there are 'too many' enemies on the scene.

        //--- Lisbeth: I will also assume that you would normally create a pool of enemies, instead of
        // instantiating and destroying them continously

        if (_enemySpawnCooldown <= 0)
        {
            // Search for a random point in the given boundary, until it is far enough from the player and is not overlapping with any obstacle
            Vector2 spawnPoint = Vector2.zero;

            //--- Lisbeth: You could have create the float here as well, so the system doesn't have to
            // possibly create 20 floats for the Garbage Collector within one frame.
            // And that is assuming this method isn't run multiple times over several frames, because the
            // conditions keeps failing.

            for (int i = 0; i < 20; i++)
            {
                //--- Lisbeth: It is generally best to avoid 'new' and do caches/temp caches instead.
                spawnPoint = new Vector2(Random.Range(_mapBounds[0].x, _mapBounds[1].x), Random.Range(_mapBounds[0].y, _mapBounds[1].y));
                float distance = Vector2.Distance(spawnPoint, _playerTransform.position);

                //--- Lisbeth: Physics are heavy. I would not even proceed to this step without checking the distance first.
                if (!Physics2D.OverlapCircle(spawnPoint, 1f, _obstacleLayer) && distance > _minimumPlayerDistance)
                {
                    break;
                }
            }
            // Initialize and pass necessary variables to the enemy object
            GameObject enemyGO = Instantiate(_enemyPrefabs.SelectRandom(), spawnPoint, Quaternion.identity, _enemyTransform);
            enemyGO.GetComponent<EnemyController>().Setup(_playerTransform);

            _enemySpawnCooldown = Random.Range(_enemySpawnTime.x, _enemySpawnTime.y);

            //--- Lisbeth: The one thing I'm missing the most, is a fail safe, which would guarentee
            // that the enemy was able to spawn somewhere, and not have a theoretical possibiliy of
            // always failing.
            // One idea to be considered, would be to divide the map up into a grid[][], which was
            // created on either starting the level, or pre-calculated before building the game,
            // where all unavailable fields (i.e. those which overlaps with obstacles) would be
            // removed, and then you could random select [x][y], based on a certain distance from
            // the player.
        }
        else
        {
            _enemySpawnCooldown -= Time.deltaTime;
        }

        // Visualize the available spawn area
        Debug.DrawLine(_mapBounds[0], new Vector2(_mapBounds[0].x, _mapBounds[1].y), Color.white);
        Debug.DrawLine(_mapBounds[0], new Vector2(_mapBounds[1].x, _mapBounds[0].y), Color.white);
        Debug.DrawLine(_mapBounds[1], new Vector2(_mapBounds[0].x, _mapBounds[1].y), Color.white);
        Debug.DrawLine(_mapBounds[1], new Vector2(_mapBounds[1].x, _mapBounds[0].y), Color.white);
        ExtensionMethods.DrawEllipse(_playerTransform.position, _minimumPlayerDistance, _minimumPlayerDistance, Color.white);
    }

    /// <summary>
    /// Executes logic to start the game.
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1f;
        _playerTransform.gameObject.SetActive(true);

        MenuManager.Instance.CloseMenuScreen();
    }

    /// <summary>
    /// Executes logic at the end of the game.
    /// </summary>
    public void EndGame()
    {
        _isGameOver = true;
        MenuManager.Instance.OpenEndScreen();
    }

    /// <summary>
    /// Resets the scene and reloads the default variables.
    /// </summary>
    public void RestartGame()
    {
        //--- Lisbeth: I would highly suggest making a 'Reset()' method, instead of loading the
        // the same scene again. I do assume though, that you would normally have done that,
        // and only chose to reload the scene since this is a relatively simple game, which is
        // also only a trial.

        SceneManager.LoadScene(0);
    }

    //--- Lisbeth: Consider creating a separate script to handle Editor only features/tools.
    // Separating this logic will make it less cumbersome to manage later, when you have
    // forgotten where the code was implemented.
    // I assume you chose to implement OnDrawGizmos and Debug.DrawLine in this script to
    // reduce production time though, and would normally consider splitting it up.

    private void OnDrawGizmos()
    {
        // If there are patrol points, visualize them
        if (_defaultPatrolPoints != null)
        {
            foreach (Vector2 patrolPoint in _defaultPatrolPoints)
            {
                Gizmos.DrawSphere(patrolPoint, 0.5f);
            }
        }
    }
}
