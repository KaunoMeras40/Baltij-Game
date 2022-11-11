using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponentInParent<EnemyV1_Attack>())
        {
            GetComponentInParent<EnemyV1_Attack>().Damage(other);
        }
        
    }

}
