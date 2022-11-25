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

    private float RangeChangeRate = 1f;
    private float RangeChangeTimer;

    public bool attacking;

    [SerializeField] private Transform ExplosionParticles;
    [SerializeField] private GameObject HitEffect;
    [SerializeField] private GameObject ammoBox;

    [HideInInspector] public bool hasAttacked;
    [HideInInspector] public bool soundplayed;

    public enemyType type;

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
        RangeChangeTimer += Time.deltaTime;
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

        if (RangeChangeTimer > RangeChangeRate && type == enemyType.Shooter)
        {
            attackRange = Random.Range(8.5f, 11.5f);
            RangeChangeTimer = 0f;
            agent.stoppingDistance = attackRange;
        }

        currentState.UpdateState(this);
    }

    public void Shoot()
    {
        GetComponent<EnemyShoot>().Shoot();
    }

    public void Damage(Collider other)
    {
        if (attacking && hasAttacked == false)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                hasAttacked = true;
                int damage = GetComponent<CharacterStats>().damage.GetValue();
                player.GetComponent<CharacterStats>().TakeDamage(damage);
                Shake(2f, 0.3f, 20f);
                Instantiate(HitEffect, player.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<AudioSource>().Play();
            }
        }
    }
    public void Explode()
    {
        if (Vector3.Distance(player.position, transform.position) < agent.stoppingDistance && !hasAttacked)
        {
            hasAttacked = true;
            int damage = GetComponent<CharacterStats>().damage.GetValue();
            player.GetComponent<CharacterStats>().TakeDamage(damage);
            Shake(2f, 0.3f, 20f);
            Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
            GetComponent<CharacterStats>().TakeDamage(1000);
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
    }

    void AttackEnd()
    {
        attacking = false;
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

}

public enum enemyType { Basic, Bomber, Bandit, Shooter, Kid }
