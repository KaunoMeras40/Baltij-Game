using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    [SerializeField] private GameObject[] Hairs;
    [SerializeField] private GameObject[] Shirts;
    [SerializeField] private GameObject[] Pants;
    [SerializeField] private GameObject[] Belts;
    [SerializeField] private GameObject[] Hats;

    [SerializeField] private GameObject[] Weapons;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipWeapon(string weapon)
    {
        foreach (GameObject item in Weapons)
        {
            if (item.name == weapon)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    public void RandomAppearance()
    {
        int hairIndex = Random.Range(0, Hairs.Length);
        int shirtIndex = Random.Range(0, Shirts.Length);
        int pantIndex = Random.Range(0, Pants.Length);
        int beltIndex = Random.Range(0, Belts.Length);
        int hatIndex = Random.Range(0, Hats.Length);

        foreach (GameObject item in Hairs)
        {
            if (item == Hairs[hairIndex])
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        foreach (GameObject item in Shirts)
        {
            if (item == Shirts[shirtIndex])
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        foreach (GameObject item in Pants)
        {
            if (item == Pants[pantIndex])
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        foreach (GameObject item in Belts)
        {
            if (item == Belts[beltIndex])
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        foreach (GameObject item in Hats)
        {
            if (item == Hats[hatIndex])
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

}
