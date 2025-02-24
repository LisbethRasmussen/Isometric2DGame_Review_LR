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

        float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected override void Update()
    {
        base.Update();

        if (Vector2.Distance(_startPosition, transform.position) >= _weaponData.Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}

