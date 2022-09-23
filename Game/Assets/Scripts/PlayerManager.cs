using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] TextMeshProUGUI moneyAmount;

    public int playerMoney;

    private void Start()
    {
        playerMoney = 20;
    }

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

    public void AddMoney(int amount)
    {
        playerMoney += amount;
    }

    private void Update()
    {
        if (currentWeapon != null)
        {
            weaponName.text = currentWeapon.itemName;
        }
        moneyAmount.text = "Money: " + playerMoney + "€";
    }



}
