using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
    float timer;

    [SerializeField] private GameObject impactParticles;
    [SerializeField] private AudioClip impact_clip;
    [SerializeField] private GameObject hitEffect;

    public int damage;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToDestroy) Destroy(gameObject);
    }

    void Impact(GameObject other)
    {
        if (other.GetComponent<CharacterJoint>()) return;
        if (other.GetComponent<CharacterStats>())
        {
            ParticleSystem ps2 = Instantiate(hitEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main2 = ps2.main;
            main2.simulationSpeed = 2;
            ps2.Play();
            ps2.GetComponent<AudioSource>().Play();
            float amount = damage * CharacterModifiers.instance.LifestealModifier.GetValue();
            if(amount > 0)
            {
                // Lifesteal
                PlayerAimController.instance.AddHealth(Mathf.RoundToInt(amount));
            }
            int ran = Random.Range(1, 100);
            if (ran <= CharacterModifiers.instance.CriticalStrikeModifier.GetValue() * 100f)
            {
                // Critical Strike
                float dmg = damage * 1.5f;
                damage = Mathf.RoundToInt(dmg);
            }
            other.GetComponent<CharacterStats>().TakeDamage(damage);
            if (other.GetComponent<CharacterStats>().IsPlayer == true)
            {
                other.GetComponent<CharacterStats>().Shake(2f, 0.3f, 20f);
            }
        }
        else
        {
            ParticleSystem ps = Instantiate(impactParticles, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = ps.main;
            main.simulationSpeed = 2;
            ps.Play();
            ps.GetComponent<AudioSource>().Play();
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Impact(collision.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Impact(other.gameObject);
    }
}
