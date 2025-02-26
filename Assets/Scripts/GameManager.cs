using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Vector2[] _defaultPatrolPoints;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private LayerMask _entityLayer;
    
    public Vector2[] DefaultPatrolPoints => _defaultPatrolPoints;
    public LayerMask ObstacleLayer => _obstacleLayer;
    public LayerMask EntityLayer => _entityLayer;

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
