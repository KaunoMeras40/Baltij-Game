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

    public bool invincible;

    public AudioSource HitSoundSource;

    NavMeshAgent agent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        dead = false;
        health = Mathf.RoundToInt(maxHealth.GetValue());
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
                float amount = 0.5f + CharacterModifiers.instance.HealModifier.GetValue();
                health += Mathf.RoundToInt(amount);
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
        if (invincible == true) return;
        float defence = (float)damage / 100f * (float)armor.GetValue();
        damage -= Mathf.RoundToInt(defence);
        health -= damage;
        animator.SetTrigger("Hit");
        float random = Random.Range(0, 3);
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
            animator.SetLayerWeight(2, 1f);
            //agent.isStopped = true;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void HitEnd()
    {
        animator.SetLayerWeight(2, 0f);
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
        dead = true;
        DeathEvent.Invoke();
        if (IsPlayer)
        {

        }
        else
        {
            PlayerManager.Instance.AddMoney(Random.Range(1, 3));
            gameObject.tag = "Untagged";
            agent.enabled = false;
            animator.enabled = false;
            GetComponent<EnemyStateManager>().OnDeath();
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

    public void AddMaxHealth(int amount)
    {
        maxHealth.AddModifier(amount);
        health = maxHealth.GetValue();
        UpdateBars();
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
