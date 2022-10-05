using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyV4_Shoot : MonoBehaviour
{

    Animator animator;
    [SerializeField] float attackRate;
    float attackRateTimer;

    bool attacking;
    bool hasAttacked;

    public float attackRange;

    [SerializeField] private GameObject HitEffect;

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private ParticleSystem ShootingEffect;

    [SerializeField] private LayerMask bulletLayer;

    [Range(0.0f, 1.0f)]
    public float HitAccuracy = 0.5f;

    Transform player;
    NavMeshAgent agent;
    CharacterStats charStats;

    public Transform aimPosition;
    [SerializeField] private LayerMask colliderMask;

    bool canSeePlayer;


    private void Start()
    {
        player = PlayerManager.Instance.player.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        charStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        attackRateTimer += Time.deltaTime;
        Ray ray = new Ray(shootingPoint.position, shootingPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit raycasthit, 999f, colliderMask))
        {
            aimPosition.position = raycasthit.point;
            if (raycasthit.transform.GetComponent<PlayerAimController>())
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
    }

    public void Shoot()
    {
        if (canSeePlayer == false) return;
        animator.SetBool("Running", false);
        animator.SetBool("Walking", false);
        if (attackRateTimer > attackRate && !attacking)
        {
            float random = Random.Range(0.0f, 1.0f);

            bool isHit = random > 1.0f - HitAccuracy;
            GameObject currentBullet = Instantiate(BulletPrefab, shootingPoint.position, Quaternion.identity);
            currentBullet.layer = 8;
            currentBullet.GetComponent<Bullet>().damage = charStats.damage.GetValue();
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            ParticleSystem.MainModule main = ShootingEffect.main;
            main.simulationSpeed = 2;
            ShootingEffect.Play();
            animator.SetLayerWeight(1, 1f);
            if (isHit)
            {
                rb.AddForce(shootingPoint.forward * 300f, ForceMode.Impulse);
            }
            else
            {
                Vector3 newPoint = shootingPoint.forward + new Vector3(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
                rb.AddForce(newPoint * 300f, ForceMode.Impulse);
            }

            GetComponent<EnemyMovement>().attacking = true;
            attackRateTimer = 0f;
            animator.SetTrigger("Attack");
        }
    }

    public void AttackEndDebug()
    {
        attacking = false;
        hasAttacked = false;
        GetComponent<EnemyMovement>().attacking = false;
        animator.SetLayerWeight(1, 0f);
    }

    void AttackStart()
    {
        attacking = true;
    }

    void AttackEnd()
    {
        attacking = false;
        hasAttacked = false;
        animator.SetLayerWeight(1, 0f);
    }
}
