using UnityEngine;

public abstract class EnemyState
{
    public abstract void EnterState(EnemyStateManager enemyState);
    public abstract void UpdateState(EnemyStateManager enemyState);
}
