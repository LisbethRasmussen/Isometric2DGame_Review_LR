using UnityEngine;

/// <summary>
/// Serves as a base class for in-game objects, moving in the isometric space.
/// Should be extended by specific controllers that require movement logic in an isometric world.
/// </summary>
public abstract class IsometricController : MonoBehaviour
{
    /// <summary>
    /// Constant that defines the isometric translation factor, used for translating the direction of movement from regular to isometric space.
    /// </summary>
    public const float IsometricTranslation = 0.5f;

    /// <summary>
    /// Constant that ensures that the movement is consistent with the isometric projection.
    /// </summary>
    public const float VelocityScale = 0.66f;

    [Header("Movement")]
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected float _moveSpeed;

    protected Vector2 _moveDirection;

    protected virtual void FixedUpdate()
    {
        HandleMovement();
    }

    /// <summary>
    /// Calculates the movement direction and applies it to the Rigidbody2D component to move the object.
    /// </summary>
    protected void HandleMovement()
    {
        // Scale the direction to the circumference of an ellipse, so the object doesn't seem to move faster vertically
        Vector2 direction = _moveDirection.ScaleToEllipse(1f, VelocityScale);
        _rb.linearVelocity = direction * _moveSpeed;

        // Visualize the velocity of the object
        float debugSize = 0.5f;
        Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0f) * _moveSpeed * debugSize);
        ExtensionMethods.DrawEllipse(transform.position, _moveSpeed * debugSize, VelocityScale * _moveSpeed * debugSize, Color.white);
    }
}
