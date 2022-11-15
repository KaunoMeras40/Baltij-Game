using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [HideInInspector] public Transform player;

    public EnemyState currentState;
    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public AttackState attackState = new AttackState();

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool canPatrol;

    public float attackRate;
    public float attackRateTimer;

    public bool attacking;

    void Start()
    {
        player = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentState = chaseState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        attackRateTimer += Time.deltaTime;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (canPatrol)
        {
            SwitchState(patrolState);
        }
        else
        {
            if (playerInSightRange && !playerInAttackRange)
            {
                if (currentState != chaseState)
                {
                    SwitchState(chaseState);
                }
            }
            else if(playerInSightRange && playerInAttackRange)
            {
                if (currentState != attackState)
                {
                    SwitchState(attackState);
                }
            }
        }
        currentState.UpdateState(this);

    }

    public void Damage(Collider other)
    {
        if (attacking)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                int damage = GetComponent<CharacterStats>().damage.GetValue();
                player.GetComponent<CharacterStats>().TakeDamage(damage);
                GetComponent<EnemyMovement>().Shake(2f, 0.3f, 20f);
                //Instantiate(HitEffect, player.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<AudioSource>().Play();
            }
        }
    }

    public void SwitchState(EnemyState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void AttackEndDebug()
    {
        attacking = false;
    }

    void AttackStart()
    {
        attacking = true;
        Debug.Log("START");
    }

    void AttackEnd()
    {
        attacking = false;
        Debug.Log("END");
    }


}
