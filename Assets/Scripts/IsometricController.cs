using UnityEngine;

public abstract class IsometricController : MonoBehaviour
{
    public const float IsometricTranslation = 0.5f;
    public const float VelocityScale = 0.66f;

    [Header("Movement")]
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected float _moveSpeed;

    protected Vector2 _moveDirection;

    private void Update()
    {
        HandleInput();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    protected void HandleMovement()
    {
        Vector2 direction = _moveDirection.ScaleToEllipse(1f, VelocityScale);
        _rb.linearVelocity = direction * _moveSpeed;

        Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0f) * 2f);
        ExtensionMethods.DrawEllipse(transform.position, 2f, VelocityScale * 2f, Color.white);
    }

    protected abstract void HandleInput();
    protected abstract void HandleAnimation();
}
