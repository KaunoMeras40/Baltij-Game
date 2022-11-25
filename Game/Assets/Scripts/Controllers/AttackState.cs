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



        if (stateManager.playerInAttackRange)
        {
            StopWalking(stateManager);
            if (ThirdPersonController.instance.moving == true)
            {
                if (stateManager.type != enemyType.Shooter)
                {
                    stateManager.animator.SetLayerWeight(1, Mathf.Lerp(stateManager.animator.GetLayerWeight(1), 1f, Time.deltaTime * 5f));
                    stateManager.animator.SetBool("Running", true);
                }
            }
        }
        

       
        if (stateManager.type == enemyType.Basic)
        {
            BasicAttack(stateManager);
        }
        else if (stateManager.type == enemyType.Shooter)
        {
            stateManager.Shoot();
            if (Vector3.Distance(stateManager.player.position, stateManager.transform.position) <= stateManager.agent.stoppingDistance)
            {
                StopWalking(stateManager);
            }
        }
    }

    void StopWalking(EnemyStateManager stateManager)
    {
        stateManager.animator.SetLayerWeight(1, Mathf.Lerp(stateManager.animator.GetLayerWeight(1), 1f, Time.deltaTime * 5f));
        stateManager.animator.SetBool("Running", false);
        stateManager.animator.SetBool("Walking", false);
    }

    public void BasicAttack(EnemyStateManager stateManager)
    {
        if (stateManager.attackRateTimer > stateManager.attackRate && !stateManager.attacking)
        {
            stateManager.hasAttacked = false;
            float random = Random.Range(0f, 2f);
            stateManager.animator.SetFloat("AttackType", random);
            stateManager.attackRateTimer = 0f;
            stateManager.animator.SetTrigger("Attack");
        }
    }



}
