using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ProjectileController : IsometricController
{
    private WeaponData _weaponData;
    private Vector2 _startPosition;

    public void Setup(Vector2 moveDirection, WeaponData weaponData)
    {
        _moveDirection = moveDirection;
        _weaponData = weaponData;
        _startPosition = transform.position;

        float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 localScale = transform.localScale;
        localScale.y = Mathf.Abs(localScale.y) * Mathf.Sign(_moveDirection.x);
        transform.localScale = localScale;
    }

    protected void Update()
    {
        if (Vector2.Distance(_startPosition, transform.position) >= _weaponData.Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isObstacle = GameManager.Instance.ObstacleLayer == (GameManager.Instance.ObstacleLayer | (1 << collision.gameObject.layer));
        if (collision.TryGetComponent(out EntityController entityController))
        {
            if (entityController.EntityData.Team != _weaponData.Entity.EntityData.Team)
            {
                Vector2 contactPoint = (transform.position + collision.transform.position) / 2f;
                entityController.TakeDamage(_weaponData.Damage, contactPoint);
                Destroy(gameObject);
            }
        }
        else if (isObstacle)
        {
            Destroy(gameObject);
        }
    }
}

