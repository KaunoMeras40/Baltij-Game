using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyV1_Attack : MonoBehaviour
{

    Animator animator;
    [SerializeField] float attackRate;
    float attackRateTimer;

    bool attacking;
    bool hasAttacked;

    public float attackRange;

    [SerializeField] private GameObject HitEffect;

    Transform player;
    NavMeshAgent agent;

    private void Start()
    {
        player = PlayerManager.Instance.player.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        attackRateTimer += Time.deltaTime;
        if (attacking && !hasAttacked)
        {
            if (Vector3.Distance(transform.position, player.position) < attackRange)
            {
                int damage = GetComponent<CharacterStats>().damage.GetValue();
                player.GetComponent<CharacterStats>().TakeDamage(damage);
                GetComponent<EnemyMovement>().Shake(2f, 0.3f, 20f);
                Instantiate(HitEffect, player.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<AudioSource>().Play();
                hasAttacked = true;
            }
        }
    }

    public void Attack()
    {
        animator.SetBool("Running", false);
        animator.SetBool("Walking", false);
        if (attackRateTimer > attackRate && !attacking)
        {
            GetComponent<EnemyMovement>().attacking = true;
            float random = Random.Range(0f, 2f);
            animator.SetFloat("AttackType", random);
            attackRateTimer = 0f;
            animator.SetTrigger("Attack");
           // Invoke(nameof(AttackEndDebug), 0.5f);
        }
    }

    public void AttackEndDebug()
    {
        attacking = false;
        hasAttacked = false;
        GetComponent<EnemyMovement>().attacking = false;
    }

    void AttackStart()
    {
        attacking = true;
    }

    void AttackEnd()
    {
        attacking = false;
        hasAttacked = false;
    }
}
