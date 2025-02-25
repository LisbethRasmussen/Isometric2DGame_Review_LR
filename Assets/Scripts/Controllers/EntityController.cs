using UnityEngine;

public abstract class EntityController : IsometricController
{
    [SerializeField] private EntityData _entityData;

    public EntityData EntityData => _entityData;

    public void ChangeFacing(float direction)
    {
        if (direction != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * (direction < 0f ? -1 : 1);
            transform.localScale = localScale;
        }
    }

    public void TakeDamage(int damage)
    {
        _entityData.Health -= damage;
    }
}
