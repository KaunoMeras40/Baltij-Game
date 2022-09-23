using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{

    [SerializeField] private int amountToRefill;

    public void setAmount(int amount)
    {
        amountToRefill = amount;
    }

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
