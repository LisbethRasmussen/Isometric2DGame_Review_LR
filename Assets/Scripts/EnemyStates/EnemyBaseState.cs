using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyController _enemyController;

    protected EnemyBaseState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }

    public abstract void EnterState();
    public abstract void UpdateState();

    public abstract void HandleInput();
    public abstract void HandleAnimation();
}
