/// <summary>
/// Serves as a base class for the states used by the enemy state machine.
/// Should be implemented to extend the behaviour of enemies.
/// </summary>
public abstract class EnemyBaseState
{
    protected EnemyController _enemyController;

    protected EnemyBaseState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }

    /// <summary>
    /// Runs logic when the state machine enters the state.
    /// Should be implemented to initialize variables used by the state.
    /// </summary>
    public abstract void EnterState();

    /// <summary>
    /// Runs logic every frame, while the state is used by the state machine.
    /// Should be implemented to run the logic of the state and to update the state of the state machine.
    /// </summary>
    public abstract void UpdateState();

    /// <summary>
    /// Handles the input logic used by the state.
    /// Should be implemented to update variables used by other behaviours.
    /// </summary>
    public abstract void HandleInput();

    /// <summary>
    /// Handles the animation corresponding to the state.
    /// Should be implemented to update visuals based on the state.
    /// </summary>
    public abstract void HandleAnimation();
}
