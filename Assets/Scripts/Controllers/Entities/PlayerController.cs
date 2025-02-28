using UnityEngine;

/// <summary>
/// Controls player behaviour and handles input made by the user.
/// </summary>
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

    /// <summary>
    /// Handles input made by the user using Unity's New Input System.
    /// </summary>
    protected override void HandleInput()
    {
        // Read the input values from the input system
        Vector2 moveInput = _playerControls.Player.Move.ReadValue<Vector2>();
        _lookDirection = _playerControls.Player.Look.ReadValue<Vector2>();
        _isAttacking = _playerControls.Player.Attack.ReadValue<float>() > 0.5f;

        // Convert the standard 2D input into isometric directions
        Vector2 horizontal = new Vector2(1f, -IsometricTranslation) * moveInput.x;
        Vector2 vertical = new Vector2(1f, IsometricTranslation) * moveInput.y;
        _moveDirection = horizontal + vertical;

        // Change the player's equipment based on the input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _equipmentIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _equipmentIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _equipmentIndex = 2;
        }
    }

    /// <summary>
    /// Handles the animation of the player based on the user's input.
    /// </summary>
    protected override void HandleAnimation()
    {
        // Change the animation based on the equipment
        _animator.SetInteger("Equipment", _equipmentIndex);

        // Convert the mouse position into world space and make the player face towards it
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(_lookDirection);
        float direction = mousePosition.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.1f)
        {
            ChangeFacing(direction);
        }
    }

    /// <summary>
    /// Run game ending logic upon the player's death.
    /// </summary>
    protected override void HandleDeath()
    {
        // Create a death particle for visual cues
        GameObject particleGO = Instantiate(GameManager.Instance.DeathParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particleGO, 1f);

        // Disable the player instead of destroying it, so other objects can still reference without crashing the game
        gameObject.SetActive(false);

        GameManager.Instance.EndGame();
    }

    /// <summary>
    /// Handles combat logic based on the equipped weapon.
    /// </summary>
    private void HandleAttack()
    {
        // If the user is attacking and a weapon is selected
        if (_isAttacking && _equipmentIndex != 0)
        {
            // Find the corresponding weapon controller to the equipment
            WeaponController weaponController = _weaponControllers[_equipmentIndex - 1];

            // Calculate the origin and direction of the attack
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(_lookDirection);
            Vector3 contactPoint = weaponController.ContactPoint.position;
            Vector2 attackDirection = mousePosition - new Vector2(contactPoint.x, contactPoint.y);

            // Assign the parameters to the weapon controller and perform the attack action
            weaponController.AttackDirection = attackDirection;
            weaponController.HandleAttack();
        }
    }

    /// <summary>
    /// Changes the currently equipped weapon of the player.
    /// </summary>
    /// <param name="equipmentIndex">The index of the desired weapon in the weapon controller collection.</param>
    public void ChangeEquipment(int equipmentIndex)
    {
        _equipmentIndex = equipmentIndex;
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
