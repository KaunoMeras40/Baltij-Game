using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;
using UnityEngine.AI;
public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;

   // public event System.Action<int, int> OnHealthChanged;
    public int health { get; private set; }
    public float lives { get; private set; }

    public Stat damage;
    public Stat armor;

    Animator animator;

    public UnityEvent DeathEvent;
    public UnityEvent HitEvent;
    public GameObject DeathEffect;

    public bool IsPlayer;

    public bool gotHit;
    public bool dead { get; private set; }

    public AudioSource HitSoundSource;

    NavMeshAgent agent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        dead = false;
        health = maxHealth.GetValue();
        if (IsPlayer)
        {
           // healthbar.SetMaxHealth(maxHealth.GetValue());
            StartCoroutine(RegenerateHealthOverTime());
        }
        else
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private IEnumerator RegenerateHealthOverTime()
    {
        while (true)
        {
            if (health < maxHealth.GetValue())
            {
                health += 1;
               // healthbar.SetCurrentHealth(health);
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                yield return null;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        float defence = (float)damage / 100f * (float)armor.GetValue();
        damage -= (int)defence;
        health -= damage;
        animator.SetTrigger("Hit");
        float random = Random.Range(0f, 2f);
        animator.SetFloat("HitType", random);
        //Debug.Log(transform.name + " takes " + damage + " damage.");
        gotHit = true;
        HitEvent.Invoke();
        if (IsPlayer)
        {
            //FindObjectOfType<AudioManager>().PlayRandomHitSound(HitSoundSource);
            //healthbar.SetCurrentHealth(health);
        }
        else
        {
            agent.isStopped = true;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void HitEnd()
    {
        agent.isStopped = false;
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

    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
        dead = true;
        animator.SetTrigger("Dead");
        DeathEvent.Invoke();
        if (IsPlayer)
        {

        }
        else
        {
            agent.enabled = false;
            animator.enabled = false;
            GetComponent<EnemyMovement>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            Instantiate(DeathEffect, transform.position, Quaternion.identity, transform);
            Destroy(gameObject, 10f);
        }
    }

    void UpdateBars()
    {
    }

    public void AddDamage(int damageModifier)
    {
        damage.AddModifier(damageModifier);
    }
    public void RemoveDamage(int damageModifier)
    {
        damage.RemoveModifier(damageModifier);
    }
    public void AddArmor(int armorModifier)
    {
        armor.AddModifier(armorModifier);
    }
    public void RemoveArmor(int armorModifier)
    {
        armor.RemoveModifier(armorModifier);
    }
    public void NPCHealthModifier(int healthModifier)
    {
        maxHealth.AddModifier(healthModifier);
        health = maxHealth.GetValue();
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health > maxHealth.GetValue())
        {
            health = maxHealth.GetValue();
        }
        UpdateBars();
    }

}
