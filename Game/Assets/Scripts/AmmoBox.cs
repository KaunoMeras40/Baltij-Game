using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{

    [SerializeField] private int amountToRefill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WeaponAmmo ammo = other.gameObject.GetComponentInChildren<WeaponAmmo>();
            if (ammo)
            {
                ammo.RefillAmmo(amountToRefill);
                Destroy(gameObject);
            }
        }
    }

}
