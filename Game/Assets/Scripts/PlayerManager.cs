using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public GameObject player;

    [SerializeField] private Item currentWeapon;

    public new List<GameObject> weaponList;

    public void switchWeapon(Item weapon)
    {
        currentWeapon = weapon;
        foreach (GameObject item in weaponList)
        {
            item.SetActive(true);
            WeaponManager manager = item.GetComponent<WeaponManager>();

            if (manager.item == weapon.item)
            {
                item.SetActive(true);
                player.GetComponent<PlayerAimController>().SwitchWeapon(weapon.pistol);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }


}
