using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyV2_Attack : MonoBehaviour
{

    [SerializeField] private float explosionRadius;
    Transform player;

    bool hasExploded = false;

    [SerializeField] private Transform ExplosionParticles;
    [SerializeField] private float soundRange;

    bool soundplayed = false;

    void Start()
    {
        player = PlayerManager.Instance.player.transform;
    }

    private void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < soundRange && !hasExploded && !soundplayed)
        {
            GetComponent<AudioSource>().Play();
            soundplayed = true;
        }
    }

    public void Explode()
    {
        if (Vector3.Distance(player.position,transform.position) < explosionRadius && !hasExploded)
        {
            int damage = GetComponent<CharacterStats>().damage.GetValue();
            player.GetComponent<CharacterStats>().TakeDamage(damage);
            GetComponent<EnemyMovement>().Shake(2f, 0.3f, 20f);
            hasExploded = true;
            Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
            GetComponent<CharacterStats>().TakeDamage(1000);
        }
    }

}
