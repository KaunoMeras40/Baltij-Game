using UnityEngine;

public class ChaseState : EnemyState
{
    public override void EnterState(EnemyStateManager stateManager)
    {
        Debug.Log("Chasing");
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        if (Vector3.Distance(stateManager.transform.position, stateManager.player.position) > stateManager.agent.stoppingDistance)
        {
            stateManager.agent.SetDestination(stateManager.player.position);
            stateManager.animator.SetBool("Running", true);
            stateManager.animator.SetBool("Walking", false);
            stateManager.animator.SetLayerWeight(1, Mathf.Lerp(stateManager.animator.GetLayerWeight(1), 0f, Time.deltaTime * 5f));
        }
    }
}
