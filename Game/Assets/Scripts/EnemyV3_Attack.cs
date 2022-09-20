using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyV3_Attack : MonoBehaviour
{
    [SerializeField] private float jumpAttackRange;
    [SerializeField] private float regularAttackRange;
    bool jumping = false;
    public AnimationCurve HeightCurve;
    public float JumpSpeed = 1;

    Animator animator;
    [SerializeField] float attackRate;
    float attackRateTimer;

    bool attacking;
    bool hasAttacked;

    public float attackRange;

    [SerializeField] private GameObject HitEffect;

    Transform player;
    NavMeshAgent agent;
    PlayerAimController aimController;
    EnemyMovement movement;

    private void Start()
    {
        player = PlayerManager.Instance.player.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        aimController = PlayerAimController.instance;
        movement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        attackRateTimer += Time.deltaTime;
        if (attacking && !hasAttacked)
        {
            agent.SetDestination(transform.position);
            if (Vector3.Distance(transform.position, player.position) < attackRange)
            {
                int damage = GetComponent<CharacterStats>().damage.GetValue();
                player.GetComponent<CharacterStats>().TakeDamage(damage);
                GetComponent<EnemyMovement>().Shake(2f, 0.3f, 20f);
                Instantiate(HitEffect, player.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<AudioSource>().Play();
                hasAttacked = true;
            }
        }
        if (Vector3.Distance(transform.position, player.position) < 2.8f)
        {
            attackRange = regularAttackRange;
            movement.attackRange = regularAttackRange;
        }
        else
        {
            attackRange = jumpAttackRange;
            movement.attackRange = jumpAttackRange;
        }

        }

    public void Attack()
    {
        if (attackRateTimer > attackRate && !attacking && !jumping)
        {
            attackRateTimer = 0f;
            animator.SetBool("Running", false);
            animator.SetBool("Walking", false);
            animator.SetTrigger("Attack");
            if (Vector3.Distance(transform.position, player.position) > 2.8f)
            {
                agent.isStopped = true;
                animator.SetFloat("AttackType", 2f);
            }
            else
            {
                attackRateTimer = 0f;
                float random = Random.Range(0f, 1f);
                animator.SetFloat("AttackType", random);
            }
        }
    }

    public void AttackEndDebug()
    {
        attacking = false;
        hasAttacked = false;
        movement.attacking = false;

    }

    void AttackStart()
    {
        movement.attacking = true;
        attacking = true;
    }

    void AttackEnd()
    {
        Debug.Log("AttackEnd");
        if (!jumping)
        {
            attacking = false;
            hasAttacked = false;
        }
        movement.attacking = false;
        attackRateTimer = 0f;
        attacking = false;
        hasAttacked = false;
        jumping = false;
        StopCoroutine(JumpAttack());
        agent.isStopped = false;
    }

    void JumpStart()
    {
        StartCoroutine(JumpAttack());
    }

    private IEnumerator JumpAttack()
    {
        agent.isStopped = true;
        jumping = true;
        Vector3 startingPosition = transform.position;

        for (float time = 0; time < 1; time += Time.deltaTime * JumpSpeed)
        {
            transform.position = Vector3.Lerp(startingPosition, aimController.jumpOffset.position, time) + Vector3.up * HeightCurve.Evaluate(time);
            transform.LookAt(player);
            yield return null;
            if (Vector3.Distance(transform.position, player.position) < 1.2f)
            {
                jumping = false;
                break;
            }
        }
        agent.isStopped = false;

        //if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 1f, agent.areaMask))
        //{
        //    agent.Warp(hit.position);
        //}

        // IsActivating = false;
    }
}