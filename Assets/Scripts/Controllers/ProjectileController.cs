using UnityEngine;

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
        if (collision.TryGetComponent(out EntityController entityController))
        {
            if (entityController.EntityData.Team != _weaponData.Entity.EntityData.Team)
            {
                Vector2 contactPoint = (transform.position + collision.transform.position) / 2f;
                entityController.TakeDamage(_weaponData.Damage, contactPoint);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

