using UnityEngine;

public class PlayerController : IsometricController
{
    [Header("Animation")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Attack")]
    [SerializeField] private int _equipmentIndex;

    private PlayerInputActions _playerControls;
    private Vector2 _lookDirection;
    private bool _isAttacking;

    private void Awake()
    {
        _playerControls = new PlayerInputActions();
    }

    protected override void HandleInput()
    {
        Vector2 _moveInput = _playerControls.Player.Move.ReadValue<Vector2>();
        Vector2 horizontal = new Vector2(1f, -IsometricTranslation) * _moveInput.x;
        Vector2 vertical = new Vector2(1f, IsometricTranslation) * _moveInput.y;

        _moveDirection = horizontal + vertical;
        _lookDirection = _playerControls.Player.Look.ReadValue<Vector2>();
        _isAttacking = _playerControls.Player.Attack.ReadValue<float>() > 0.5f;
    }

    protected override void HandleAnimation()
    {
        _animator.SetInteger("Equipment", _equipmentIndex);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(_lookDirection);
        float direction = mousePosition.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.1f)
        {
            _spriteRenderer.flipX = direction < 0f;
        }
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
