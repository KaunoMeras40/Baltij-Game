using UnityEngine;
using StarterAssets;
public class AttackState : EnemyState
{

    public override void EnterState(EnemyStateManager stateManager)
    {

    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        Debug.Log("ATTACK STATE");
        stateManager.agent.SetDestination(stateManager.player.position);
        stateManager.transform.LookAt(stateManager.player);
        if (ThirdPersonController.instance.moving == true)
        {
            stateManager.animator.SetLayerWeight(1, Mathf.Lerp(stateManager.animator.GetLayerWeight(1), 1f, Time.deltaTime * 5f));
            stateManager.animator.SetBool("Running", true);
        }
        else
        {
            if (stateManager.playerInAttackRange)
            {
                stateManager.animator.SetBool("Running", false);
                stateManager.animator.SetBool("Walking", false);
            }
        }
        if (stateManager.attacking)
        {

        }
        
        Attack(stateManager);
    }

    void Attack(EnemyStateManager stateManager)
    {
        if (stateManager.attackRateTimer > stateManager.attackRate && !stateManager.attacking)
        {
            float random = Random.Range(0f, 2f);
            stateManager.animator.SetFloat("AttackType", random);
            stateManager.attackRateTimer = 0f;
            stateManager.animator.SetTrigger("Attack");
        }
    }

}
