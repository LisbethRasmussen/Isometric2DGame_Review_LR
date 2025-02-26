using UnityEngine;

public abstract class EntityController : IsometricController
{
    [SerializeField] private EntityData _entityData;
    [SerializeField] private HealthBarController _healthBarController;

    public EntityData EntityData => _entityData;

    private int _maxHealth;

    protected virtual void Start()
    {
        _maxHealth = _entityData.Health;
    }

    protected virtual void Update()
    {
        HandleInput();
        HandleAnimation();
    }

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
        _healthBarController.UpdateHealth((float)_entityData.Health / _maxHealth);
        if (_entityData.Health <= 0)
        {
            HandleDeath();
        }
    }

    protected abstract void HandleInput();
    protected abstract void HandleAnimation();
    protected abstract void HandleDeath();
}
