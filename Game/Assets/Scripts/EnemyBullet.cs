using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
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
            other.GetComponent<CharacterStats>().TakeDamage(damage);
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
