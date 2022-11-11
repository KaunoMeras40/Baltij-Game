using UnityEngine;

public class ChaseState : EnemyState
{
    public override void EnterState(EnemyStateManager stateManager)
    {
        Debug.Log("Chasing");
        throw new System.NotImplementedException();
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        if (Vector3.Distance(stateManager.transform.position, stateManager.player.position) > stateManager.agent.stoppingDistance)
        {
            stateManager.agent.SetDestination(stateManager.player.position);
            stateManager.animator.SetBool("Running", true);
            stateManager.animator.SetBool("Walking", false);
        }
        throw new System.NotImplementedException();
    }
}
