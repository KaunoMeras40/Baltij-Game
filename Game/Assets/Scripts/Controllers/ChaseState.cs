using UnityEngine;

public class ChaseState : EnemyState
{
    public override void EnterState(EnemyStateManager stateManager)
    {
        Debug.Log("Chasing");
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        Debug.Log("CHASE STATE");
        if (Vector3.Distance(stateManager.transform.position, stateManager.player.position) > stateManager.agent.stoppingDistance)
        {
            stateManager.agent.SetDestination(stateManager.player.position);
            stateManager.animator.SetBool("Running", true);
            stateManager.animator.SetBool("Walking", false);
            stateManager.animator.SetLayerWeight(1, Mathf.Lerp(stateManager.animator.GetLayerWeight(1), 0f, Time.deltaTime * 5f));
        }

        if (stateManager.type == enemyType.Bomber)
        {
            if (Vector3.Distance(stateManager.player.position, stateManager.transform.position) < stateManager.agent.stoppingDistance + 2 && !stateManager.soundplayed)
            {
                stateManager.GetComponent<AudioSource>().Play();
                stateManager.soundplayed = true;
            }
            stateManager.Explode();
        }
    }
}
