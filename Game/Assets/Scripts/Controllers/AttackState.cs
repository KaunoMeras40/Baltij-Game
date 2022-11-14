using UnityEngine;

public class AttackState : EnemyState
{

    bool attac = false;
    public override void EnterState(EnemyStateManager stateManager)
    {
        Debug.Log("Attacking");
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        Debug.Log("ATTACK STATE");
       if (attac == false)
        {
            attac = true;
            Attack(stateManager);
        }    
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
