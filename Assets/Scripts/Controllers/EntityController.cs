using UnityEngine;

/// <summary>
/// Serves as a base class for all in-game entities, providing core functionalities and abstract methods for custom behaviour.
/// Should be extended for entities, that will fight and/or die during the game.
/// </summary>
public abstract class EntityController : IsometricController
{
    [SerializeField] private EntityData _entityData;
    [SerializeField] private HealthBarController _healthBarController;

    private float _maxHealth;
    
    #region Variable Getters
    public EntityData EntityData => _entityData;
    #endregion

    protected virtual void Start()
    {
        _maxHealth = _entityData.Health;
    }

    protected virtual void Update()
    {
        HandleInput();
        HandleAnimation();

        HandleHealthBar();
    }

    /// <summary>
    /// Changes the scale of the entity to face towards a given direction.
    /// </summary>
    /// <param name="direction">The direction the object should face.</param>
    public void ChangeFacing(float direction)
    {
        if (direction != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(direction);
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// Applies damage to the entity and handles death event.
    /// </summary>
    /// <param name="damage">The amount of damage inflicted.</param>
    /// <param name="hitPoint">The point where the visual cue of the damage will appear.</param>
    public void TakeDamage(float damage, Vector2 hitPoint)
    {
        // Subtract damage from the entity's health and trigger death handling logic if needed
        _entityData.Health -= damage;
        if (_entityData.Health <= 0)
        {
            HandleDeath();
        }
        else
        {
            // Create a blood particle for visual cues
            GameObject particleGO = Instantiate(GameManager.Instance.BloodParticlePrefab, hitPoint, Quaternion.identity, transform);
            Destroy(particleGO, 1f);
        }
    }

    /// <summary>
    /// Manages the entity's health and synchronizes it with the health bar.
    /// </summary>
    private void HandleHealthBar()
    {
        // Match the scale of the health bar to the entity's scale, so it doesn't look flipped
        Vector3 healthBarScale = _healthBarController.transform.localScale;
        healthBarScale.x = Mathf.Sign(transform.localScale.x);
        _healthBarController.transform.localScale = healthBarScale;

        // If the entity is alive and not fully healed, apply healing and update the health bar
        if (_entityData.Health > 0 && _entityData.Health < _maxHealth)
        {
            _entityData.Health += _entityData.HealingRate * Time.deltaTime;
            if (_entityData.Health > _maxHealth)
            {
                _entityData.Health = _maxHealth;
            }
        }
        _healthBarController.UpdateHealth(_entityData.Health / _maxHealth);
    }

    /// <summary>
    /// Processes input used by other behaviours of the entity.
    /// Should be implemented to handle movement, attacks, or other interactions.
    /// </summary>
    protected abstract void HandleInput();

    /// <summary>
    /// Manages the animator and the visual representation of the entity.
    /// Should synchronize animations with the entity's current state and actions.
    /// </summary>
    protected abstract void HandleAnimation();

    /// <summary>
    /// Executes logic upon the death of the entity.
    /// Should handle clean-up, trigger death animations, and notify other systems if needed.
    /// </summary>
    protected abstract void HandleDeath();
}
