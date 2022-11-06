using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ammoAmounts
{
    public int Light;
    public int Medium;
    public int Heavy;
    public int Slugs;
}

public class AmmoBox : MonoBehaviour
{
    public ammoAmounts ammoAmount;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WeaponAmmo ammo = other.gameObject.GetComponentInChildren<WeaponAmmo>();
            if (ammo)
            {
                if (ammo.ammoType == ammoTypes.Light)
                {
                    ammo.RefillAmmo(ammoAmount.Light);
                }
                else if (ammo.ammoType == ammoTypes.Medium)
                {
                    ammo.RefillAmmo(ammoAmount.Medium);
                }
                else if (ammo.ammoType == ammoTypes.Heavy)
                {
                    ammo.RefillAmmo(ammoAmount.Heavy);
                }
                else if (ammo.ammoType == ammoTypes.Slugs)
                {
                    ammo.RefillAmmo(ammoAmount.Slugs);
                }
                Destroy(gameObject);
            }
        }
    }
}
