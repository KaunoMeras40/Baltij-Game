using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private Animator animator;

    public UnityEvent attackEvent;

    public bool attacking;

    [SerializeField] private bool canPatrol;

    [SerializeField] GameObject ammoBox;
    private void Start()
    {
        player = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();

        if (attacking)
        {
            agent.SetDestination(player.position);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 5f));
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 5f));
        }

        if (Vector3.Distance(transform.position, player.position) < agent.stoppingDistance)
        {
            animator.SetBool("Running", false);
            animator.SetBool("Walking", false);
        }

        if (Vector3.Distance(transform.position, player.position) < attackRange && attacking == false)
        {
            transform.LookAt(player);
            attackEvent.Invoke();
        }
        else if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            if (canPatrol == false)
            {
                ChasePlayer();
            }
            else
            {
                Patroling();
            }
        }

        }

    private void Patroling()
    {
        if(agent.isStopped == false)
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            animator.SetBool("Running", false);
            animator.SetBool("Walking", true);

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        } 
    }

    public void OnDeath()
    {
        int ran = Random.Range(1, 100);
        if (ran < 6f)
        {
            Instantiate(ammoBox, transform.position, Quaternion.identity);
        }
        this.enabled = false;
    }
    void AttackEnd()
    {
        attacking = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (!attacking)
        {
            if (Vector3.Distance(transform.position, player.position) > agent.stoppingDistance)
            {
                agent.SetDestination(player.position);
                animator.SetBool("Running", true);
                animator.SetBool("Walking", false);
            }
        }
    }
    public void Shake(float intensity, float time, float fr)
    {
        GameObject[] ShakeObjects = GameObject.FindGameObjectsWithTag("Shake");
        foreach (GameObject obj in ShakeObjects)
        {
            if (obj.GetComponent<CameraShake>() != null)
            {
                obj.GetComponent<CameraShake>().SetCameraShake(intensity, time, fr);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
