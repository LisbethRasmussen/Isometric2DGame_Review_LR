using UnityEngine;

public class PlayerController : EntityController
{
    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("Attack")]
    [SerializeField] private int _equipmentIndex;
    [SerializeField] private WeaponController[] _weaponControllers;

    private PlayerInputActions _playerControls;
    private Vector2 _lookDirection;
    private bool _isAttacking;

    private void Awake()
    {
        _playerControls = new PlayerInputActions();
    }

    protected override void Update()
    {
        base.Update();

        HandleAttack();
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
            ChangeFacing(direction);
        }
    }

    private void HandleAttack()
    {
        if (_isAttacking && _equipmentIndex != 0)
        {
            WeaponController weaponController = _weaponControllers[_equipmentIndex - 1];
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(_lookDirection);
            Vector3 contactPoint = weaponController.ContactPoint.position;
            Vector2 attackDirection = mousePosition - new Vector2(contactPoint.x, contactPoint.y);

            weaponController.AttackDirection = attackDirection;
            weaponController.HandleAttack();
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
