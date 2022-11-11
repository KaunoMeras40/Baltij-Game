using UnityEngine;

public class PatrolState : EnemyState
{
    private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public override void EnterState(EnemyStateManager stateManager)
    {
        Debug.Log("Patroling");
        throw new System.NotImplementedException();
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        if (stateManager.agent.isStopped == false)
        {
            if (!walkPointSet) SearchWalkPoint(stateManager);

            if (walkPointSet)
                stateManager.agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = stateManager.transform.position - walkPoint;
            stateManager.animator.SetBool("Running", false);
            stateManager.animator.SetBool("Walking", true);

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }

        throw new System.NotImplementedException();
    }

    private void SearchWalkPoint(EnemyStateManager stateManager)
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(stateManager.transform.position.x + randomX, stateManager.transform.position.y, stateManager.transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -stateManager.transform.up, 2f, stateManager.whatIsGround))
            walkPointSet = true;
    }

}
