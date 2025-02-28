using UnityEngine;

/// <summary>
/// Controller script for the projectiles shot by the entities.
/// </summary>
public class ProjectileController : IsometricController
{
    private WeaponData _weaponData;
    private Vector2 _startPosition;

    /// <summary>
    /// Initializes the state of the projectile and sets up the parameters for moving it.
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <param name="weaponData"></param>
    public void Setup(Vector2 moveDirection, WeaponData weaponData)
    {
        // Store the parameters
        _moveDirection = moveDirection;
        _weaponData = weaponData;
        _startPosition = transform.position;

        // Rotate the projectile towards the direction it's moving
        float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip the projectile on the Y axis in case it was rotated by more than 180°
        Vector3 localScale = transform.localScale;
        localScale.y = Mathf.Abs(localScale.y) * Mathf.Sign(_moveDirection.x);
        transform.localScale = localScale;
    }

    protected void Update()
    {
        // If the projectile travelled further, than the range of the weapon, destroy it
        if (Vector2.Distance(_startPosition, transform.position) >= _weaponData.Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision happened with a wall or obstacle
        bool isObstacle = GameManager.Instance.ObstacleLayer == (GameManager.Instance.ObstacleLayer | (1 << collision.gameObject.layer));

        // If the collision was made with a rival entity, damage it and destroy the projectile
        if (collision.TryGetComponent(out EntityController entityController))
        {
            if (entityController.EntityData.Team != _weaponData.Entity.EntityData.Team)
            {
                Vector2 contactPoint = (transform.position + collision.transform.position) / 2f;
                entityController.TakeDamage(_weaponData.Damage, contactPoint);
                Destroy(gameObject);
            }
        }
        // If the collision was made with an obstacle, destroy the projectile
        else if (isObstacle)
        {
            Destroy(gameObject);
        }
    }
}

