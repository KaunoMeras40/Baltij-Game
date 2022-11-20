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
           // stateManager.animator.SetLayerWeight(1, Mathf.Lerp(stateManager.animator.GetLayerWeight(1), 1f, Time.deltaTime * 5f));
            stateManager.animator.SetBool("Running", true);
        }
        else
        {
            if (stateManager.playerInAttackRange)
            {
                //stateManager.animator.SetLayerWeight(1, Mathf.Lerp(stateManager.animator.GetLayerWeight(1), 1f, Time.deltaTime * 5f));
                stateManager.animator.SetBool("Running", false);
                stateManager.animator.SetBool("Walking", false);
            }
        }
        
       
        if (stateManager.type == enemyType.Basic)
        {
            BasicAttack(stateManager, false);
        }
        else if (stateManager.type == enemyType.Mutant)
        {
            if (Vector3.Distance(stateManager.player.position, stateManager.transform.position) > stateManager.agent.stoppingDistance + 1.5f)
            {
                Debug.Log("BIGGER");
                if (Vector3.Distance(stateManager.player.position, stateManager.transform.position) < stateManager.attackRange)
                {
                    BasicAttack(stateManager, true);
                    Debug.Log("JUMP");
                }
            }
            else if (Vector3.Distance(stateManager.player.position, stateManager.transform.position) <= stateManager.agent.stoppingDistance)
            {
                BasicAttack(stateManager, false);
                Debug.Log("BASIC");
            }
        }
    }

    public void MutantAttack(EnemyStateManager stateManager)
    {
        if (stateManager.attackRateTimer > stateManager.attackRate && !stateManager.attacking && !stateManager.jumping)
        {
            stateManager.attackRateTimer = 0f;
            stateManager.animator.SetTrigger("Attack");
            if (Vector3.Distance(stateManager.transform.position, stateManager.player.position) > 2.8f)
            {
                stateManager.agent.isStopped = true;
                stateManager.animator.SetFloat("AttackType", 2f);
            }
            else
            {
                float random = Random.Range(0f, 1f);
                stateManager.animator.SetFloat("AttackType", random);
            }
        }
    }

    public void BasicAttack(EnemyStateManager stateManager, bool jump)
    {
        if (stateManager.attackRateTimer > stateManager.attackRate && !stateManager.attacking)
        {
            stateManager.hasAttacked = false;
            float random = Random.Range(0f, 2f);
            if (jump)
            {
                random = 2f;
                Debug.Log("2");
            }
            stateManager.animator.SetFloat("AttackType", random);
            stateManager.attackRateTimer = 0f;
            stateManager.animator.SetTrigger("Attack");
        }
    }



}
