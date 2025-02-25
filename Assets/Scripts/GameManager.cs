using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Vector2[] _defaultPatrolPoints;
    
    public Vector2[] DefaultPatrolPoints => _defaultPatrolPoints;

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
